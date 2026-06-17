using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.SalesInvoice;
using Pharmacy.Application.DTOs.SalesInvoice;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.SalesInvoice;

/// <summary>
/// Handler for creating a new Sales Invoice (POS transaction)
/// This creates the invoice, items, and corresponding stock OUT transactions
/// </summary>
public class CreateSalesInvoiceHandler : IRequestHandler<CreateSalesInvoiceCommand, SalesInvoiceDto>
{
    private readonly ISalesInvoiceRepository _invoiceRepository;
    private readonly ISalesInvoiceItemRepository _itemRepository;
    private readonly IStockTransactionRepository _transactionRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IAppLookupDetailRepository _lookupRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IInvoiceNumberService _invoiceNumberService;
    private readonly IOfferDetailRepository _offerDetailRepository;
    private readonly IJournalPostingService _journalPostingService;
    private readonly IFiscalYearRepository _fiscalYearRepository;
    private readonly ISalesInvoicePaymentRepository _paymentRepository;
    private readonly ICashierShiftDetailRepository _shiftDetailRepository;
    private readonly IMapper _mapper;

    public CreateSalesInvoiceHandler(
        ISalesInvoiceRepository invoiceRepository,
        ISalesInvoiceItemRepository itemRepository,
        IStockTransactionRepository transactionRepository,
        IStockRepository stockRepository,
        IProductRepository productRepository,
        IBranchRepository branchRepository,
        IAppLookupDetailRepository lookupRepository,
        ICustomerRepository customerRepository,
        IInvoiceNumberService invoiceNumberService,
        IOfferDetailRepository offerDetailRepository,
        IJournalPostingService journalPostingService,
        IFiscalYearRepository fiscalYearRepository,
        ISalesInvoicePaymentRepository paymentRepository,
        ICashierShiftDetailRepository shiftDetailRepository,
        IMapper mapper)
    {
        _invoiceRepository    = invoiceRepository;
        _itemRepository       = itemRepository;
        _transactionRepository = transactionRepository;
        _stockRepository      = stockRepository;
        _productRepository    = productRepository;
        _branchRepository     = branchRepository;
        _lookupRepository     = lookupRepository;
        _customerRepository   = customerRepository;
        _invoiceNumberService = invoiceNumberService;
        _offerDetailRepository = offerDetailRepository;
        _journalPostingService  = journalPostingService;
        _fiscalYearRepository   = fiscalYearRepository;
        _paymentRepository      = paymentRepository;
        _shiftDetailRepository  = shiftDetailRepository;
        _mapper                 = mapper;
    }

    public async Task<SalesInvoiceDto> Handle(CreateSalesInvoiceCommand request, CancellationToken cancellationToken)
    {
        // Validate branch exists
        var branch = await _branchRepository.GetByIdAsync(request.Invoice.BranchId, cancellationToken);
        if (branch == null)
        {
            throw new KeyNotFoundException($"Branch with ID '{request.Invoice.BranchId}' not found");
        }

        // Pre-flight accounting validation (always, regardless of AutoPostJournal)
        // Ensures accounts are configured before any sale is saved when auto-post is on
        if (branch.AutoPostJournal)
            await _journalPostingService.ValidateSalesAccountingSetupAsync(request.Invoice.BranchId, cancellationToken);

        // Validate all products exist and have sufficient stock
        foreach (var item in request.Invoice.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID '{item.ProductId}' not found");
            }

            if (!await _stockRepository.HasSufficientStockAsync(
                item.ProductId, 
                request.Invoice.BranchId, 
                item.Quantity,
                item.BatchNumber,
                cancellationToken))
            {
                throw new InvalidOperationException($"Insufficient stock for product '{product.DrugName}'");
            }
        }

        // Get lookup values
        var invoiceStatuses = await _lookupRepository.GetByMasterCodeAsync("INVOICE_STATUS", cancellationToken);
        var completedStatus = invoiceStatuses.FirstOrDefault(s => s.ValueCode == "COMPLETED");
        
        var transactionTypes = await _lookupRepository.GetByMasterCodeAsync("TRANSACTION_TYPE", cancellationToken);
        var outType = transactionTypes.FirstOrDefault(t => t.ValueCode == "OUT");

        // Generate invoice number from InvoiceSetup table (atomic increment)
        var invoiceNumber = await _invoiceNumberService.GenerateNextAsync(
            request.Invoice.BranchId,
            IInvoiceNumberService.LookupDetailPosInvoiceId,
            cancellationToken);

        // ── Resolve customer ──────────────────────────────────────────────
        // Priority: explicit CustomerId > convenience name/phone fields > default Cash Patient
        Guid? customerId = request.Invoice.CustomerId;

        if (customerId == null && !string.IsNullOrWhiteSpace(request.Invoice.CustomerPhone))
        {
            var existing = await _customerRepository.GetByPhoneAsync(request.Invoice.CustomerPhone, cancellationToken);
            customerId = existing?.Oid;
        }

        if (customerId == null && !string.IsNullOrWhiteSpace(request.Invoice.CustomerName))
        {
            var newCustomer = new Domain.Entities.Customer
            {
                NameEN     = request.Invoice.CustomerName,
                Phone      = request.Invoice.CustomerPhone,
                Email      = request.Invoice.CustomerEmail,
                CreatedAt  = DateTime.UtcNow
            };
            await _customerRepository.AddAsync(newCustomer, cancellationToken);
            customerId = newCustomer.Oid;
        }

        if (customerId == null)
        {
            var walkIn = await _customerRepository.GetDefaultWalkInAsync(cancellationToken);
            customerId = walkIn?.Oid;
        }
        // Calculate totals
        decimal subTotal = 0;
        var invoiceItems = new List<SalesInvoiceItem>();
        // Extra free-item lines generated by FREE_ITEMS offers
        var freeItemLines = new List<SalesInvoiceItem>();

        foreach (var itemDto in request.Invoice.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId, cancellationToken);
            var unitPrice = itemDto.UnitPrice ?? product!.Price ?? 0;

            decimal itemDiscountAmount = 0;
            decimal totalPrice;
            Guid? offerDetailId = null;
            string? offerNameSnapshot = null;

            // ── Apply offer if provided ───────────────────────────────────
            if (itemDto.OfferDetailId.HasValue)
            {
                var offerDetail = await _offerDetailRepository.GetByIdAsync(itemDto.OfferDetailId.Value, cancellationToken);

                if (offerDetail != null && offerDetail.OfferMaster != null)
                {
                    var offerTypeCode = offerDetail.OfferMaster.OfferType?.ValueCode;
                    offerDetailId = offerDetail.Oid;
                    offerNameSnapshot = offerDetail.OfferMaster.OfferNameEn;

                    switch (offerTypeCode)
                    {
                        // ── DISCOUNT: apply percent or fixed amount ───────
                        case "DISCOUNT":
                            if (offerDetail.DiscountPercent.HasValue)
                            {
                                itemDiscountAmount = unitPrice * itemDto.Quantity * offerDetail.DiscountPercent.Value / 100;
                            }
                            else if (offerDetail.DiscountAmount.HasValue)
                            {
                                itemDiscountAmount = offerDetail.DiscountAmount.Value * itemDto.Quantity;
                            }
                            totalPrice = (unitPrice * itemDto.Quantity) - itemDiscountAmount;
                            break;

                        // ── PACKAGE_PRICE: fixed total for N units ────────
                        case "PACKAGE_PRICE":
                            if (offerDetail.PackageQuantity.HasValue && offerDetail.PackagePrice.HasValue
                                && itemDto.Quantity >= offerDetail.PackageQuantity.Value)
                            {
                                var packages = Math.Floor(itemDto.Quantity / offerDetail.PackageQuantity.Value);
                                var remainder = itemDto.Quantity % offerDetail.PackageQuantity.Value;
                                totalPrice = (packages * offerDetail.PackagePrice.Value) + (remainder * unitPrice);
                                itemDiscountAmount = (unitPrice * itemDto.Quantity) - totalPrice;
                            }
                            else
                            {
                                totalPrice = unitPrice * itemDto.Quantity;
                            }
                            break;

                        // ── FREE_ITEMS: buy N get M free ──────────────────
                        case "FREE_ITEMS":
                            totalPrice = unitPrice * itemDto.Quantity;
                            if (offerDetail.BuyQuantity.HasValue && offerDetail.FreeQuantity.HasValue
                                && itemDto.Quantity >= offerDetail.BuyQuantity.Value)
                            {
                                var sets = Math.Floor(itemDto.Quantity / offerDetail.BuyQuantity.Value);
                                var freeQty = sets * offerDetail.FreeQuantity.Value;
                                var freeProductId = offerDetail.FreeProductId ?? itemDto.ProductId;

                                // Add a zero-price free-item line
                                freeItemLines.Add(new SalesInvoiceItem
                                {
                                    ProductId         = freeProductId,
                                    Quantity          = (decimal)freeQty,
                                    RemainingQuantity = (decimal)freeQty,
                                    ReturnedQuantity  = 0,
                                    UnitPrice         = 0,
                                    CostPrice         = 0,
                                    DiscountPercent   = 100,
                                    DiscountAmount    = 0,
                                    TaxPercent        = 0,
                                    TaxAmount         = 0,
                                    NetPrice          = 0,
                                    TotalPrice        = 0,
                                    IsFreeItem        = true,
                                    OfferDetailId     = offerDetailId,
                                    OfferNameSnapshot = $"{offerNameSnapshot} (Free)",
                                    Notes             = $"Free item from offer: {offerNameSnapshot}",
                                    CreatedAt         = DateTime.UtcNow
                                });
                            }
                            break;

                        default:
                            itemDiscountAmount = itemDto.DiscountPercent.HasValue
                                ? (unitPrice * itemDto.Quantity * itemDto.DiscountPercent.Value / 100)
                                : 0;
                            totalPrice = (unitPrice * itemDto.Quantity) - itemDiscountAmount;
                            break;
                    }
                }
                else
                {
                    // Offer not found — fall back to manual discount
                    itemDiscountAmount = itemDto.DiscountPercent.HasValue
                        ? (unitPrice * itemDto.Quantity * itemDto.DiscountPercent.Value / 100)
                        : 0;
                    totalPrice = (unitPrice * itemDto.Quantity) - itemDiscountAmount;
                }
            }
            else
            {
                // No offer — use manual discount from DTO
                itemDiscountAmount = itemDto.DiscountPercent.HasValue
                    ? (unitPrice * itemDto.Quantity * itemDto.DiscountPercent.Value / 100)
                    : 0;
                totalPrice = (unitPrice * itemDto.Quantity) - itemDiscountAmount;
            }

            var effectiveTaxPercent = itemDto.TaxPercent ?? request.Invoice.TaxPercent;
            var computedItemTaxAmount = effectiveTaxPercent.HasValue
                ? Math.Round(totalPrice * effectiveTaxPercent.Value / 100, 2)
                : 0m;
            var itemTaxAmount = itemDto.TaxAmount ?? computedItemTaxAmount;

            var invoiceItem = new SalesInvoiceItem
            {
                ProductId         = itemDto.ProductId,
                Quantity          = itemDto.Quantity,
                RemainingQuantity = itemDto.Quantity,
                ReturnedQuantity  = 0,
                UnitPrice         = unitPrice,
                CostPrice         = itemDto.CostPrice,
                DiscountPercent   = itemDto.DiscountPercent,
                DiscountAmount    = itemDiscountAmount,
                NetPrice          = totalPrice,
                TaxPercent        = effectiveTaxPercent,
                TaxAmount         = itemTaxAmount,
                TotalPrice        = totalPrice + itemTaxAmount,
                LineNumber        = itemDto.LineNumber ?? 0,
                BatchNumber       = itemDto.BatchNumber,
                SerialNumber      = itemDto.SerialNumber,
                ExpiryDate        = itemDto.ExpiryDate,
                IsFreeItem        = false,
                Notes             = itemDto.Notes,
                OfferDetailId     = offerDetailId,
                OfferNameSnapshot = offerNameSnapshot,
                CreatedAt         = DateTime.UtcNow
            };

            invoiceItems.Add(invoiceItem);
            subTotal += invoiceItem.NetPrice ?? 0;
        }

        // Calculate invoice totals
        var invoiceDiscountAmount = request.Invoice.DiscountPercent.HasValue 
            ? (subTotal * request.Invoice.DiscountPercent.Value / 100) 
            : 0;
        var subTotalAfterDiscount = subTotal - invoiceDiscountAmount;
        // Use explicit TaxAmount override when provided; otherwise sum line-level tax amounts
        var invoiceTaxAmount = request.Invoice.TaxAmount.HasValue
            ? request.Invoice.TaxAmount.Value
            : invoiceItems.Sum(i => i.TaxAmount ?? 0);
        var totalAmount = subTotalAfterDiscount + invoiceTaxAmount;

        // Create invoice
        var invoice = new Domain.Entities.SalesInvoice
        {
            InvoiceNumber      = invoiceNumber,
            BranchId           = request.Invoice.BranchId,
            CustomerId         = customerId,
            SubTotal           = subTotal,
            DiscountPercent    = request.Invoice.DiscountPercent,
            DiscountAmount     = invoiceDiscountAmount,
            TaxPercent         = request.Invoice.TaxPercent,
            TaxAmount          = invoiceTaxAmount,
            TotalAmount        = totalAmount,
            PaidAmount         = request.Invoice.PaidAmount,
            ChangeAmount       = request.Invoice.ChangeAmount,
            InvoiceDate        = request.Invoice.InvoiceDate ?? DateTime.UtcNow,
            PaymentMethodId    = request.Invoice.PaymentMethodId,
            InvoiceStatusId    = completedStatus?.Oid,
            CashierId          = request.Invoice.CashierId,
            DoctorId           = request.Invoice.DoctorId,
            PrescriptionNumber = request.Invoice.PrescriptionNumber,
            DoctorName         = request.Invoice.DoctorName,
            Notes              = request.Invoice.Notes
        };

        // Resolve active fiscal year for journal posting
        var fiscalYear = await _fiscalYearRepository.GetCurrentAsync(cancellationToken);
        invoice.FiscalYearId = fiscalYear?.Oid;

        // If explicit payment lines are provided, derive PaidAmount from their sum
        if (request.Invoice.Payments.Count > 0)
            invoice.PaidAmount = request.Invoice.Payments.Sum(p => p.Amount);

        var createdInvoice = await _invoiceRepository.AddAsync(invoice, cancellationToken);

        // Persist payment lines atomically with the invoice
        foreach (var paymentDto in request.Invoice.Payments)
        {
            await _paymentRepository.AddAsync(new Domain.Entities.SalesInvoicePayment
            {
                SalesInvoiceId  = createdInvoice.Oid,
                ShiftId         = paymentDto.ShiftId,
                PaymentMethodId = paymentDto.PaymentMethodId,
                Amount          = paymentDto.Amount,
                ReferenceNumber = paymentDto.ReferenceNumber,
                TransactionId   = paymentDto.TransactionId,
                ApprovalCode    = paymentDto.ApprovalCode,
                PaymentDate     = paymentDto.PaymentDate,
                Notes           = paymentDto.Notes,
                CreatedAt       = DateTime.UtcNow
            }, cancellationToken);

            // Persist CashierShiftDetail record if ShiftId is provided
            if (paymentDto.ShiftId.HasValue)
            {
                await _shiftDetailRepository.AddAsync(new Domain.Entities.CashierShiftDetail
                {
                    ShiftId       = paymentDto.ShiftId.Value,
                    TransactionDate = paymentDto.PaymentDate,
                    TransactionTypeId = null,  // Optional: can be used to categorize shift transactions
                    ReferenceId   = createdInvoice.Oid,
                    ReferenceNumber = invoiceNumber,
                    PaymentMethodId = paymentDto.PaymentMethodId,
                    Amount        = paymentDto.Amount,
                    Notes         = $"Sale Invoice #{invoiceNumber}",
                    CreatedAt     = DateTime.UtcNow
                }, cancellationToken);
            }
        }

        // Create invoice items and stock transactions
        int lineCounter = 1;
        foreach (var item in invoiceItems)
        {
            item.InvoiceId  = createdInvoice.Oid;
            item.LineNumber = item.LineNumber > 0 ? item.LineNumber : lineCounter;
            await _itemRepository.AddAsync(item, cancellationToken);

            // Create stock OUT transaction with detail
            var stockTransaction = new Domain.Entities.StockTransaction
            {
                FromBranchId = request.Invoice.BranchId,
                ToBranchId = null,
                TransactionTypeId = outType?.Oid,
                ReferenceNumber = invoiceNumber,
                TransactionDate = DateTime.UtcNow,
                TotalValue = item.TotalPrice,
                SalesInvoiceId = createdInvoice.Oid,
                Status = "Completed",
                Notes = $"Sale - Invoice #{invoiceNumber}"
            };

            var transactionDetail = new Domain.Entities.StockTransactionDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitCost = item.CostPrice,
                TotalCost = item.Quantity * (item.CostPrice ?? 0),
                BatchNumber = item.BatchNumber,
                ExpiryDate = item.ExpiryDate,
                LineNumber = lineCounter++
            };

            stockTransaction.Details.Add(transactionDetail);
            await _transactionRepository.AddAsync(stockTransaction, cancellationToken);

            await _stockRepository.UpdateQuantityAsync(
                item.ProductId,
                request.Invoice.BranchId,
                -item.Quantity,
                item.BatchNumber,
                item.ExpiryDate,
                cancellationToken);
        }

        // Persist free-item lines generated by FREE_ITEMS offers
        foreach (var freeItem in freeItemLines)
        {
            freeItem.InvoiceId  = createdInvoice.Oid;
            freeItem.LineNumber = lineCounter++;
            await _itemRepository.AddAsync(freeItem, cancellationToken);

            // Deduct free items from stock as well
            await _stockRepository.UpdateQuantityAsync(
                freeItem.ProductId,
                request.Invoice.BranchId,
                -freeItem.Quantity,
                null,
                null,
                cancellationToken);
        }

        // Fetch the complete invoice with items
        var completeInvoice = await _invoiceRepository.GetWithItemsAsync(createdInvoice.Oid, cancellationToken);

        // ═══════════════════════════════════════════════════════════════════════
        // AUTO-POST JOURNAL ENTRY — Enhanced with VAT Classification
        // ═══════════════════════════════════════════════════════════════════════
        
        var paymentMethodCode = request.Invoice.PaymentMethodId.HasValue
            ? (await _lookupRepository.GetByIdAsync(request.Invoice.PaymentMethodId.Value, cancellationToken))?.ValueCode
            : null;

        // Build enhanced line items with VAT category classification
        var enhancedItems = invoiceItems.Select(i =>
        {
            // Determine VAT category based on TaxPercent
            var vatCategory = (i.TaxPercent ?? 0) switch
            {
                > 0 => VatCategory.Taxable,      // Standard VAT (e.g., 15%)
                0 when i.TaxPercent.HasValue => VatCategory.ZeroRated,  // Explicitly 0% VAT
                _ => VatCategory.Exempt          // No VAT at all (null)
            };

            return new Application.Interfaces.SalesInvoiceLineItem(
                ProductId:          i.ProductId,
                ProductName:        i.Product?.DrugName ?? "Unknown",
                VatCategory:        vatCategory,
                Quantity:           i.Quantity,
                UnitPrice:          i.UnitPrice ?? 0m,
                LineDiscountAmount: i.DiscountAmount ?? 0m,
                NetPrice:           i.NetPrice ?? 0m,
                TaxPercent:         i.TaxPercent ?? 0m,
                TaxAmount:          i.TaxAmount ?? 0m,
                TotalPrice:         i.TotalPrice ?? 0m,
                CostPrice:          i.CostPrice ?? 0m,
                LineNumber:         i.LineNumber,
                IsFreeItem:         i.IsFreeItem);
        }).ToList().AsReadOnly();

        // Build payment collection (currently single payment method)
        var payments = new List<Application.Interfaces.PaymentMethodDetail>();
        if (!string.IsNullOrEmpty(paymentMethodCode))
        {
            var paidAmount = request.Invoice.PaidAmount ?? totalAmount;
            payments.Add(new Application.Interfaces.PaymentMethodDetail(
                MethodCode:     paymentMethodCode,
                Amount:         paidAmount,
                BankAccountId:  null));  // TODO: Support specific bank account selection
        }

        var postingRequest = new Application.Interfaces.SalesInvoicePostingRequest(
            InvoiceOid:             createdInvoice.Oid,
            BranchId:               request.Invoice.BranchId,
            FiscalYearId:           invoice.FiscalYearId,
            InvoiceNumber:          invoiceNumber,
            InvoiceDate:            invoice.InvoiceDate ?? DateTime.UtcNow,
            Items:                  enhancedItems,
            InvoiceDiscountAmount:  invoiceDiscountAmount,
            TotalAmount:            totalAmount,
            Payments:               payments.AsReadOnly(),
            CustomerId:             customerId);

        if (branch.AutoPostJournal)
            await _journalPostingService.PostSalesInvoiceAsync(postingRequest, cancellationToken);

        return _mapper.Map<SalesInvoiceDto>(completeInvoice);
    }
}
