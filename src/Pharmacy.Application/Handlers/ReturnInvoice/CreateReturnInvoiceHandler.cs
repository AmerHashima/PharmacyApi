using AutoMapper;
using Pharmacy.Application.Commands.ReturnInvoice;
using Pharmacy.Application.DTOs.ReturnInvoice;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.ReturnInvoice;

/// <summary>
/// Handler for creating a new Return Invoice
/// This creates the return invoice, items, and corresponding stock IN transactions
/// </summary>
public class CreateReturnInvoiceHandler : IRequestHandler<CreateReturnInvoiceCommand, ReturnInvoiceDto>
{
    private readonly IReturnInvoiceRepository _returnInvoiceRepository;
    private readonly IReturnInvoiceItemRepository _returnItemRepository;
    private readonly ISalesInvoiceRepository _salesInvoiceRepository;
    private readonly ISalesInvoiceItemRepository _salesInvoiceItemRepository;
    private readonly IStockTransactionRepository _transactionRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IAppLookupDetailRepository _lookupRepository;
    private readonly IInvoiceNumberService _invoiceNumberService;
    private readonly IMapper _mapper;

    public CreateReturnInvoiceHandler(
        IReturnInvoiceRepository returnInvoiceRepository,
        IReturnInvoiceItemRepository returnItemRepository,
        ISalesInvoiceRepository salesInvoiceRepository,
        ISalesInvoiceItemRepository salesInvoiceItemRepository,
        IStockTransactionRepository transactionRepository,
        IStockRepository stockRepository,
        IProductRepository productRepository,
        IBranchRepository branchRepository,
        IAppLookupDetailRepository lookupRepository,
        IInvoiceNumberService invoiceNumberService,
        IMapper mapper)
    {
        _returnInvoiceRepository = returnInvoiceRepository;
        _returnItemRepository = returnItemRepository;
        _salesInvoiceRepository = salesInvoiceRepository;
        _salesInvoiceItemRepository = salesInvoiceItemRepository;
        _transactionRepository = transactionRepository;
        _stockRepository = stockRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _lookupRepository = lookupRepository;
        _invoiceNumberService = invoiceNumberService;
        _mapper = mapper;
    }

    public async Task<ReturnInvoiceDto> Handle(CreateReturnInvoiceCommand request, CancellationToken cancellationToken)
    {
        // Validate original invoice exists and load with items
        var originalInvoice = await _salesInvoiceRepository.GetWithItemsAsync(request.ReturnInvoice.OriginalInvoiceId, cancellationToken);
        if (originalInvoice == null)
        {
            throw new KeyNotFoundException($"Original sales invoice with ID '{request.ReturnInvoice.OriginalInvoiceId}' not found");
        }

        // Validate branch exists
        var branch = await _branchRepository.GetByIdAsync(request.ReturnInvoice.BranchId, cancellationToken);
        if (branch == null)
        {
            throw new KeyNotFoundException($"Branch with ID '{request.ReturnInvoice.BranchId}' not found");
        }

        // Validate all products exist and check RemainingQuantity on original invoice items
        foreach (var item in request.ReturnInvoice.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID '{item.ProductId}' not found");
            }

            if (item.OriginalInvoiceItemId.HasValue)
            {
                var originalItem = originalInvoice.Items
                    .FirstOrDefault(i => i.Oid == item.OriginalInvoiceItemId.Value && !i.IsDeleted);

                if (originalItem == null)
                {
                    throw new KeyNotFoundException($"Original invoice item with ID '{item.OriginalInvoiceItemId}' not found in invoice '{originalInvoice.InvoiceNumber}'");
                }

                if (originalItem.RemainingQuantity < item.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Cannot return {item.Quantity} of product '{product.DrugName}'. " +
                        $"Only {originalItem.RemainingQuantity} remaining for return on invoice '{originalInvoice.InvoiceNumber}'");
                }
            }
        }

        // Get lookup values
        var invoiceStatuses = await _lookupRepository.GetByMasterCodeAsync("INVOICE_STATUS", cancellationToken);
        var completedStatus = invoiceStatuses.FirstOrDefault(s => s.ValueCode == "COMPLETED");

        var transactionTypes = await _lookupRepository.GetByMasterCodeAsync("TRANSACTION_TYPE", cancellationToken);
        var inType = transactionTypes.FirstOrDefault(t => t.ValueCode == "IN");

        // Generate return number from InvoiceSetup table (atomic increment)
        var returnNumber = await _invoiceNumberService.GenerateNextAsync(
            request.ReturnInvoice.BranchId,
            IInvoiceNumberService.FormatReturnPosInvoice,
            cancellationToken);

        // Calculate totals
        decimal subTotal = 0;
        var returnItems = new List<ReturnInvoiceItem>();

        foreach (var itemDto in request.ReturnInvoice.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId, cancellationToken);
            var unitPrice = itemDto.UnitPrice ?? product!.Price ?? 0;
            var itemDiscount = itemDto.DiscountPercent.HasValue
                ? (unitPrice * itemDto.Quantity * itemDto.DiscountPercent.Value / 100)
                : 0;
            var totalPrice = (unitPrice * itemDto.Quantity) - itemDiscount;

            var returnItem = new ReturnInvoiceItem
            {
                OriginalInvoiceItemId = itemDto.OriginalInvoiceItemId,
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = unitPrice,
                DiscountPercent = itemDto.DiscountPercent,
                DiscountAmount = itemDiscount,
                TotalPrice = totalPrice,
                BatchNumber = itemDto.BatchNumber,
                ExpiryDate = itemDto.ExpiryDate,
                Notes = itemDto.Notes
            };

            returnItems.Add(returnItem);
            subTotal += totalPrice;
        }

        // Calculate invoice totals
        var invoiceDiscountAmount = request.ReturnInvoice.DiscountPercent.HasValue
            ? (subTotal * request.ReturnInvoice.DiscountPercent.Value / 100)
            : 0;
        var totalAmount = subTotal - invoiceDiscountAmount;

        // Create return invoice
        var returnInvoice = new Domain.Entities.ReturnInvoice
        {
            ReturnNumber = returnNumber,
            OriginalInvoiceId = request.ReturnInvoice.OriginalInvoiceId,
            BranchId = request.ReturnInvoice.BranchId,
            CustomerName = request.ReturnInvoice.CustomerName,
            CustomerPhone = request.ReturnInvoice.CustomerPhone,
            SubTotal = subTotal,
            DiscountPercent = request.ReturnInvoice.DiscountPercent,
            DiscountAmount = invoiceDiscountAmount,
            TotalAmount = totalAmount,
            RefundAmount = totalAmount,
            ReturnDate = request.ReturnInvoice.ReturnDate ?? DateTime.UtcNow,
            PaymentMethodId = request.ReturnInvoice.PaymentMethodId,
            InvoiceStatusId = completedStatus?.Oid,
            CashierId = request.ReturnInvoice.CashierId,
            ReturnReasonId = request.ReturnInvoice.ReturnReasonId,
            Notes = request.ReturnInvoice.Notes
        };

        var createdReturnInvoice = await _returnInvoiceRepository.AddAsync(returnInvoice, cancellationToken);

        // Create return invoice items, update RemainingQuantity, and stock IN transactions
        foreach (var item in returnItems)
        {
            item.ReturnInvoiceId = createdReturnInvoice.Oid;
            await _returnItemRepository.AddAsync(item, cancellationToken);

            // Reduce RemainingQuantity on the original sales invoice item
            if (item.OriginalInvoiceItemId.HasValue)
            {
                var originalItem = originalInvoice.Items
                    .First(i => i.Oid == item.OriginalInvoiceItemId.Value);
                originalItem.RemainingQuantity -= item.Quantity;
                await _salesInvoiceItemRepository.UpdateAsync(originalItem, cancellationToken);
            }

            // Create stock IN transaction for returned item
            var stockTransaction = new Domain.Entities.StockTransaction
            {
                ToBranchId = request.ReturnInvoice.BranchId,
                TransactionTypeId = inType?.Oid,
                TransactionDate = returnInvoice.ReturnDate ?? DateTime.UtcNow,
                Notes = $"Return from invoice {originalInvoice.InvoiceNumber} - Return #{returnNumber}"
            };
            await _transactionRepository.AddAsync(stockTransaction, cancellationToken);

            // Update stock - add returned quantity back
            await _stockRepository.UpdateQuantityAsync(
                item.ProductId,
                request.ReturnInvoice.BranchId,
                item.Quantity,
                item.BatchNumber,
                item.ExpiryDate,
                cancellationToken);
        }

        // Return the created return invoice with items
        var result = await _returnInvoiceRepository.GetWithItemsAsync(createdReturnInvoice.Oid, cancellationToken);
        return _mapper.Map<ReturnInvoiceDto>(result);
    }
}
