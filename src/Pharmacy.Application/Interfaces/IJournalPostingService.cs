using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Application.Interfaces;

/// <summary>
/// Reusable service that creates a balanced JournalEntry + JournalEntryDetails
/// for any source document (SalesInvoice, ReturnInvoice, Voucher, etc.).
/// </summary>
public interface IJournalPostingService
{
    /// <summary>
    /// Posts a journal entry for a sales invoice.
    /// Debits the AR/Cash account (payment method account) and credits the Sales Revenue account.
    /// </summary>
    Task<JournalEntry> PostSalesInvoiceAsync(
        Guid branchId,
        Guid? fiscalYearId,
        string invoiceNumber,
        DateTime invoiceDate,
        decimal totalAmount,
        Guid? receivableAccountId,
        Guid? revenueAccountId,
        Guid referenceId,
        string? description = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Posts a journal entry for a return invoice (reversal of sales entry).
    /// Debits the Sales Revenue account and credits the AR/Cash account.
    /// </summary>
    Task<JournalEntry> PostReturnInvoiceAsync(
        Guid branchId,
        Guid? fiscalYearId,
        string returnNumber,
        DateTime returnDate,
        decimal totalAmount,
        Guid? receivableAccountId,
        Guid? revenueAccountId,
        Guid referenceId,
        string? description = null,
        CancellationToken cancellationToken = default);
}
