using AutoMapper;
using Pharmacy.Application.Commands.SalesInvoice;
using Pharmacy.Application.DTOs.SalesInvoice;
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
    private readonly IMapper _mapper;

    public CreateSalesInvoiceHandler(
        ISalesInvoiceRepository invoiceRepository,
        ISalesInvoiceItemRepository itemRepository,
        IStockTransactionRepository transactionRepository,
        IStockRepository stockRepository,
        IProductRepository productRepository,
        IBranchRepository branchRepository,
        IAppLookupDetailRepository lookupRepository,
        IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _itemRepository = itemRepository;
        _transactionRepository = transactionRepository;
        _stockRepository = stockRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _lookupRepository = lookupRepository;
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

        // Generate invoice number
        var invoiceNumber = await _invoiceRepository.GenerateInvoiceNumberAsync(request.Invoice.BranchId, cancellationToken);

        // Calculate totals
        decimal subTotal = 0;
        var invoiceItems = new List<SalesInvoiceItem>();

        foreach (var itemDto in request.Invoice.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId, cancellationToken);
            var unitPrice = itemDto.UnitPrice ?? product!.Price ?? 0;
            var itemDiscount = itemDto.DiscountPercent.HasValue 
                ? (unitPrice * itemDto.Quantity * itemDto.DiscountPercent.Value / 100) 
                : 0;
            var totalPrice = (unitPrice * itemDto.Quantity) - itemDiscount;

            var invoiceItem = new SalesInvoiceItem
            {
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
            InvoiceNumber = invoiceNumber,
            BranchId = request.Invoice.BranchId,
            CustomerName = request.Invoice.CustomerName,
            CustomerPhone = request.Invoice.CustomerPhone,
            CustomerEmail = request.Invoice.CustomerEmail,
            SubTotal = subTotal,
            DiscountPercent = request.Invoice.DiscountPercent,
            DiscountAmount = invoiceDiscountAmount,
            TotalAmount = totalAmount,
            InvoiceDate = request.Invoice.InvoiceDate ?? DateTime.UtcNow,
            PaymentMethodId = request.Invoice.PaymentMethodId,
            InvoiceStatusId = completedStatus?.Oid,
            CashierId = request.Invoice.CashierId,
            PrescriptionNumber = request.Invoice.PrescriptionNumber,
            DoctorName = request.Invoice.DoctorName,
            Notes = request.Invoice.Notes
        };

        var createdInvoice = await _invoiceRepository.AddAsync(invoice, cancellationToken);

        // Create invoice items and stock transactions
        foreach (var item in invoiceItems)
        {
            item.InvoiceId = createdInvoice.Oid;
            await _itemRepository.AddAsync(item, cancellationToken);

            // Create stock OUT transaction
            var stockTransaction = new Domain.Entities.StockTransaction
            {
                ProductId = item.ProductId,
                FromBranchId = request.Invoice.BranchId,
                Quantity = item.Quantity,
                TransactionTypeId = outType?.Oid,
                ReferenceNumber = invoiceNumber,
                TransactionDate = DateTime.UtcNow,
                UnitCost = item.CostPrice,
                TotalValue = item.TotalPrice,
                BatchNumber = item.BatchNumber,
                ExpiryDate = item.ExpiryDate,
                SalesInvoiceId = createdInvoice.Oid,
                Notes = $"Sale - Invoice #{invoiceNumber}"
            };

            await _transactionRepository.AddAsync(stockTransaction, cancellationToken);

            // Update stock
            await _stockRepository.UpdateQuantityAsync(
                item.ProductId, 
                request.Invoice.BranchId, 
                -item.Quantity, 
                cancellationToken);
        }

        // Fetch the complete invoice with items
        var completeInvoice = await _invoiceRepository.GetWithItemsAsync(createdInvoice.Oid, cancellationToken);
        return _mapper.Map<SalesInvoiceDto>(completeInvoice);
    }
}
