using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.PurchaseInvoice;
using Pharmacy.Application.DTOs.PurchaseInvoice;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.PurchaseInvoice;

/// <summary>Creates invoice, receipt, stock, payments and journal as one transaction.</summary>
public class CreatePurchaseInvoiceHandler : IRequestHandler<CreatePurchaseInvoiceCommand, PurchaseInvoiceDto>
{
    private readonly IPurchaseInvoiceRepository _invoices;
    private readonly IStockTransactionRepository _transactions;
    private readonly IStockRepository _stock;
    private readonly IProductRepository _products;
    private readonly IBranchRepository _branches;
    private readonly IStakeholderRepository _suppliers;
    private readonly IAppLookupDetailRepository _lookups;
    private readonly IFiscalYearRepository _fiscalYears;
    private readonly IJournalPostingService _journal;
    private readonly IMapper _mapper;

    public CreatePurchaseInvoiceHandler(
        IPurchaseInvoiceRepository invoices, IStockTransactionRepository transactions,
        IStockRepository stock, IProductRepository products, IBranchRepository branches,
        IStakeholderRepository suppliers, IAppLookupDetailRepository lookups,
        IFiscalYearRepository fiscalYears, IJournalPostingService journal, IMapper mapper)
    {
        _invoices = invoices; _transactions = transactions; _stock = stock;
        _products = products; _branches = branches; _suppliers = suppliers;
        _lookups = lookups; _fiscalYears = fiscalYears; _journal = journal; _mapper = mapper;
    }

    public async Task<PurchaseInvoiceDto> Handle(CreatePurchaseInvoiceCommand request, CancellationToken ct)
    {
        var dto = request.Invoice;
        if (dto.Items.Count == 0)
            throw new InvalidOperationException("At least one purchase item is required.");

        var branch = await _branches.GetByIdAsync(dto.BranchId, ct)
            ?? throw new KeyNotFoundException($"Branch '{dto.BranchId}' not found.");
        var supplier = await _suppliers.GetByIdAsync(dto.SupplierId, ct)
            ?? throw new KeyNotFoundException($"Supplier '{dto.SupplierId}' not found.");
        if (!supplier.IsActive) throw new InvalidOperationException("The selected supplier is inactive.");
        if (branch.AutoPostJournal)
            await _journal.ValidateStockTransactionAccountingSetupAsync(branch.Oid, "IN", ct);

        var productNames = new Dictionary<Guid, string>();
        foreach (var item in dto.Items)
        {
            var product = await _products.GetByIdAsync(item.ProductId, ct)
                ?? throw new KeyNotFoundException($"Product '{item.ProductId}' not found.");
            productNames[item.ProductId] = product.DrugName ?? item.ProductId.ToString();
            if (item.ExpiryDate.Date <= dto.PurchaseDate.Date)
                throw new InvalidOperationException($"Batch '{item.BatchNumber}' is already expired.");
        }

        if (dto.DiscountPercent is < 0 or > 100)
            throw new InvalidOperationException("Purchase discount percent must be between 0 and 100.");
        if (dto.Payments.Any(p => p.Amount <= 0))
            throw new InvalidOperationException("Purchase payment amounts must be greater than zero.");

        var invoiceNumber = await GenerateNumberAsync(ct);
        var inType = (await _lookups.GetByMasterCodeAsync("TRANSACTION_TYPE", ct))
            .FirstOrDefault(t => t.ValueCode == "IN")
            ?? throw new InvalidOperationException("IN transaction type is not configured.");
        var fiscalYear = dto.FiscalYearId.HasValue
            ? await _fiscalYears.GetByIdAsync(dto.FiscalYearId.Value, ct)
            : await _fiscalYears.GetCurrentAsync(ct);

        decimal taxableNet = 0, zeroVatNet = 0, exemptNet = 0, taxTotal = 0;
        decimal grossBeforeHeaderDiscount = 0;
        var stockTx = new Domain.Entities.StockTransaction
        {
            ToBranchId = dto.BranchId, TransactionTypeId = inType.Oid,
            ReferenceNumber = invoiceNumber, TransactionDate = dto.PurchaseDate,
            SupplierId = dto.SupplierId, Status = "Completed", Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        var lineNumber = 1;
        foreach (var item in dto.Items)
        {
            var gross = item.Quantity * item.UnitCost;
            var afterFirst = gross * (1 - (item.DiscountPercentOne ?? 0) / 100m);
            var afterLineDiscounts = afterFirst * (1 - (item.DiscountPercentTwo ?? 0) / 100m);
            grossBeforeHeaderDiscount += afterLineDiscounts;
            var net = Math.Round(afterLineDiscounts * (1 - (dto.DiscountPercent ?? 0) / 100m), 2);
            var tax = Math.Round(net * (item.TaxPercent ?? 0) / 100m, 2);

            if (item.TaxPercent is > 0) { taxableNet += net; taxTotal += tax; }
            else if (item.TaxPercent.HasValue) zeroVatNet += net;
            else exemptNet += net;

            stockTx.Details.Add(new StockTransactionDetail
            {
                ProductId = item.ProductId, Quantity = item.Quantity,
                UnitCost = net / item.Quantity, NetCost = net,
                TaxPercent = item.TaxPercent, TaxAmount = tax, TotalCost = net + tax,
                DiscountPercentOne = item.DiscountPercentOne,
                DiscountPercentTwo = item.DiscountPercentTwo,
                ProductPrice = item.ProductPrice, BatchNumber = item.BatchNumber.Trim(),
                ExpiryDate = item.ExpiryDate, SerialNumber = item.SerialNumber,
                LineNumber = lineNumber++, Notes = item.Notes, CreatedAt = DateTime.UtcNow
            });
        }

        var subTotal = taxableNet + zeroVatNet + exemptNet;
        var total = subTotal + taxTotal;
        var paid = dto.Payments.Sum(p => p.Amount);
        if (paid > total) throw new InvalidOperationException("Purchase payments cannot exceed invoice total.");

        stockTx.TotalValue = total; stockTx.PayedAmount = paid; stockTx.RemainingAmount = total - paid;
        await _transactions.AddAsync(stockTx, ct);
        foreach (var detail in stockTx.Details)
            await _stock.ReceiveAsync(detail.ProductId, dto.BranchId, detail.Quantity,
                detail.UnitCost ?? 0, detail.BatchNumber!, detail.ExpiryDate!.Value, ct);

        var invoice = new Domain.Entities.PurchaseInvoice
        {
            PurchaseInvoiceNumber = invoiceNumber, SupplierInvoiceNumber = dto.SupplierInvoiceNumber,
            BranchId = dto.BranchId, SupplierId = dto.SupplierId, PurchaseDate = dto.PurchaseDate,
            StockTransactionId = stockTx.Oid, FiscalYearId = fiscalYear?.Oid,
            SubTotal = subTotal, DiscountPercent = dto.DiscountPercent,
            DiscountAmount = Math.Round(grossBeforeHeaderDiscount - subTotal, 2),
            TaxAmount = taxTotal, TotalAmount = total, PaidAmount = paid,
            InvoiceStatusId = dto.InvoiceStatusId, Notes = dto.Notes, CreatedAt = DateTime.UtcNow
        };
        var payments = dto.Payments.Select(p => new PurchaseInvoicePayment
        {
            PurchaseInvoiceId = invoice.Oid, PaymentVoucherId = p.PaymentVoucherId,
            PaymentMethodId = p.PaymentMethodId, Amount = p.Amount,
            ReferenceNumber = p.ReferenceNumber, TransactionId = p.TransactionId,
            ChequeNumber = p.ChequeNumber, PaymentDate = p.PaymentDate,
            Notes = p.Notes, CreatedAt = DateTime.UtcNow
        }).ToList();
        await _invoices.InsertMasterDetailAsync(invoice, payments, ct);

        if (branch.AutoPostJournal)
        {
            var accountingPayments = new List<PaymentMethodDetail>();
            foreach (var payment in dto.Payments)
            {
                var code = (await _lookups.GetByIdAsync(payment.PaymentMethodId, ct))?.ValueCode;
                if (!string.IsNullOrWhiteSpace(code)) accountingPayments.Add(new(code, payment.Amount, null));
            }
            var postingItems = stockTx.Details.Select(d => new StockTransactionLineItem(
                d.ProductId, productNames[d.ProductId], d.Quantity, d.UnitCost ?? 0,
                d.NetCost ?? 0, d.LineNumber)).ToList();
            var entry = await _journal.PostStockTransactionAsync(new(
                stockTx.Oid, dto.BranchId, fiscalYear?.Oid, invoiceNumber, dto.PurchaseDate,
                "IN", postingItems, dto.SupplierId, taxableNet, zeroVatNet, exemptNet,
                taxTotal, paid, accountingPayments), ct);
            invoice.JournalEntryId = entry.Oid;
            await _invoices.UpdateAsync(invoice, ct);
        }

        return _mapper.Map<PurchaseInvoiceDto>(
            await _invoices.GetWithPaymentsAsync(invoice.Oid, ct));
    }

    private async Task<string> GenerateNumberAsync(CancellationToken ct)
        => $"PI-{DateTime.UtcNow:yyyyMMdd}-{(await _invoices.CountAsync(ct) + 1):D5}";
}
