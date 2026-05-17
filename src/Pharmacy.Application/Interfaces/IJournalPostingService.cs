using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Application.Interfaces;

/// <summary>
/// Creates balanced JournalEntry records for sales and return invoices.
/// Each call wraps all inserts in a single database transaction.
/// </summary>
public interface IJournalPostingService
{
    /// <summary>
    /// Posts two journal entries for a completed sales invoice:
    ///   Entry 1 — Sales: DR Cash/Bank/Receivable + DR Discount;  CR Sales + CR VAT
    ///   Entry 2 — COGS:  DR COGS;  CR Inventory
    /// Also patches SalesInvoice.JournalEntryId inside the same transaction.
    /// </summary>
    Task<SalesInvoicePostingResult> PostSalesInvoiceAsync(
        SalesInvoicePostingRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Posts a reversing journal entry for a return invoice.
    ///   DR Sales Revenue (reversal);  CR Cash/Bank/Receivable (refund)
    /// </summary>
    Task<JournalEntry> PostReturnInvoiceAsync(
        ReturnInvoicePostingRequest request,
        CancellationToken cancellationToken = default);
}

// ─────────────────────────────────────────────────────────────────────────────
// Request / Result records
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// All data the journal posting service needs to post a sales invoice.
/// </summary>
/// <param name="InvoiceOid">PK of the already-persisted SalesInvoice row.</param>
/// <param name="PaymentMethodCode">Lookup ValueCode: "CASH" | "BANK" | "CREDIT".</param>
/// <param name="CustomerId">Required when PaymentMethodCode == "CREDIT".</param>
/// <param name="Items">Cost price + quantity per line (used for COGS entry).</param>
public record SalesInvoicePostingRequest(
    Guid InvoiceOid,
    Guid BranchId,
    Guid? FiscalYearId,
    string InvoiceNumber,
    DateTime InvoiceDate,
    decimal SubTotal,
    decimal DiscountAmount,
    decimal TaxAmount,
    decimal TotalAmount,
    string? PaymentMethodCode,
    Guid? CustomerId,
    IReadOnlyList<(decimal CostPrice, decimal Quantity)> Items);

/// <summary>Result of a successful sales invoice posting.</summary>
public record SalesInvoicePostingResult(JournalEntry SalesEntry, JournalEntry CogsEntry);

/// <summary>All data needed to post a return invoice reversal.</summary>
public record ReturnInvoicePostingRequest(
    Guid ReturnInvoiceOid,
    Guid BranchId,
    Guid? FiscalYearId,
    string ReturnNumber,
    DateTime ReturnDate,
    decimal SubTotal,
    decimal TaxAmount,
    decimal TotalAmount,
    string? PaymentMethodCode,
    Guid? CustomerId);
