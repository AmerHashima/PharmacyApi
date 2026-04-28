using AutoMapper;
using Pharmacy.Application.Commands.SalesInvoice;
using Pharmacy.Application.DTOs.SalesInvoice;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using MediatR;

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
        IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _itemRepository = itemRepository;
        _transactionRepository = transactionRepository;
        _stockRepository = stockRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _lookupRepository = lookupRepository;
        _customerRepository = customerRepository;
        _invoiceNumberService = invoiceNumberService;
        _offerDetailRepository = offerDetailRepository;
        _mapper = mapper;
    }

    public async Task<SalesInvoiceDto> Handle(CreateSalesInvoiceCommand request, CancellationToken cancellationToken)
    {
        // Validate branch exists
        var branch = await _branchRepository.GetByIdAsync(request.Invoice.BranchId, cancellationToken);
        if (branch == null)
        {
            throw new KeyNotFoundException($"Branch with ID '{request.Invoice.BranchId}' not found");
        }

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
                                    ProductId = freeProductId,
                                    Quantity = (decimal)freeQty,
                                    RemainingQuantity = (decimal)freeQty,
                                    UnitPrice = 0,
                                    DiscountPercent = 100,
                                    DiscountAmount = 0,
                                    TotalPrice = 0,
                                    OfferDetailId = offerDetailId,
                                    OfferNameSnapshot = $"{offerNameSnapshot} (Free)",
                                    Notes = $"Free item from offer: {offerNameSnapshot}",
                                    CreatedAt = DateTime.UtcNow
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

            var invoiceItem = new SalesInvoiceItem
            {
                ProductId         = itemDto.ProductId,
                Quantity          = itemDto.Quantity,
                RemainingQuantity = itemDto.Quantity,
                UnitPrice         = unitPrice,
                DiscountPercent   = itemDto.DiscountPercent,
                DiscountAmount    = itemDiscountAmount,
                TotalPrice        = totalPrice,
                BatchNumber       = itemDto.BatchNumber,
                ExpiryDate        = itemDto.ExpiryDate,
                Notes             = itemDto.Notes,
                OfferDetailId     = offerDetailId,
                OfferNameSnapshot = offerNameSnapshot,
                CreatedAt         = DateTime.UtcNow
            };

            invoiceItems.Add(invoiceItem);
            subTotal += totalPrice;
        }

        // Calculate invoice totals
        var invoiceDiscountAmount = request.Invoice.DiscountPercent.HasValue 
            ? (subTotal * request.Invoice.DiscountPercent.Value / 100) 
            : 0;
        var totalAmount = subTotal - invoiceDiscountAmount;

        // Create invoice
        var invoice = new Domain.Entities.SalesInvoice
        {
            InvoiceNumber     = invoiceNumber,
            BranchId          = request.Invoice.BranchId,
            CustomerId        = customerId,
            SubTotal          = subTotal,
            DiscountPercent   = request.Invoice.DiscountPercent,
            DiscountAmount    = invoiceDiscountAmount,
            TotalAmount       = totalAmount,
            InvoiceDate       = request.Invoice.InvoiceDate ?? DateTime.UtcNow,
            PaymentMethodId   = request.Invoice.PaymentMethodId,
            InvoiceStatusId   = completedStatus?.Oid,
            CashierId         = request.Invoice.CashierId,
            PrescriptionNumber = request.Invoice.PrescriptionNumber,
            DoctorName        = request.Invoice.DoctorName,
            Notes             = request.Invoice.Notes
        };

        var createdInvoice = await _invoiceRepository.AddAsync(invoice, cancellationToken);

        // Create invoice items and stock transactions
        int lineNumber = 1;
        foreach (var item in invoiceItems)
        {
            item.InvoiceId = createdInvoice.Oid;
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
                LineNumber = lineNumber++
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
            freeItem.InvoiceId = createdInvoice.Oid;
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
        return _mapper.Map<SalesInvoiceDto>(completeInvoice);
    }
}
