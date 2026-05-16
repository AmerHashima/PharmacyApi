using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Infrastructure.Services;

/// <summary>
/// Creates balanced JournalEntry records for source documents such as
/// SalesInvoice and ReturnInvoice.
///
/// Each posting generates:
///   - One JournalEntry master row
///   - Two JournalEntryDetail rows (debit + credit) that keep TotalDebit == TotalCredit
///
/// The EntryNumber uses the shared VoucherSequences table via IVoucherNumberService.
/// The ReferenceTypeId is resolved from AppLookupDetail (LookupCode = JOURNAL_REF_TYPE).
/// </summary>
public sealed class JournalPostingService : IJournalPostingService
{
    private readonly IJournalEntryRepository _journalRepo;
    private readonly IVoucherNumberService _numberService;

    // ValueCode constants — must match AppLookupDetail rows seeded under LookupCode="JOURNAL_REF_TYPE"
    public const string RefTypeSalesInvoice   = "SALES_INV";
    public const string RefTypeReturnInvoice  = "RETURN_INV";
    public const string TypeSalesEntry        = "SI";
    public const string TypeReturnEntry       = "RI";

    public JournalPostingService(
        IJournalEntryRepository journalRepo,
        IVoucherNumberService numberService)
    {
        _journalRepo   = journalRepo;
        _numberService = numberService;
    }

    public Task<JournalEntry> PostSalesInvoiceAsync(
        Guid branchId,
        Guid? fiscalYearId,
        string invoiceNumber,
        DateTime invoiceDate,
        decimal totalAmount,
        Guid? receivableAccountId,
        Guid? revenueAccountId,
        Guid referenceId,
        string? description = null,
        CancellationToken cancellationToken = default)
        => PostAsync(
            branchId, fiscalYearId,
            TypeSalesEntry, invoiceNumber, invoiceDate, totalAmount,
            debitAccountId:  receivableAccountId,   // DR: cash / receivable
            creditAccountId: revenueAccountId,       // CR: sales revenue
            referenceId, description, cancellationToken);

    public Task<JournalEntry> PostReturnInvoiceAsync(
        Guid branchId,
        Guid? fiscalYearId,
        string returnNumber,
        DateTime returnDate,
        decimal totalAmount,
        Guid? receivableAccountId,
        Guid? revenueAccountId,
        Guid referenceId,
        string? description = null,
        CancellationToken cancellationToken = default)
        => PostAsync(
            branchId, fiscalYearId,
            TypeReturnEntry, returnNumber, returnDate, totalAmount,
            debitAccountId:  revenueAccountId,       // DR: sales revenue (reversal)
            creditAccountId: receivableAccountId,    // CR: cash / receivable (refund)
            referenceId, description, cancellationToken);

    // ─────────────────────────────────────────────────────────────────────
    private async Task<JournalEntry> PostAsync(
        Guid branchId,
        Guid? fiscalYearId,
        string entryType,
        string referenceDocNumber,
        DateTime entryDate,
        decimal amount,
        Guid? debitAccountId,
        Guid? creditAccountId,
        Guid referenceId,
        string? description,
        CancellationToken cancellationToken)
    {
        // Generate independent JE number (e.g. 2025-BR01-JE-0000001)
        var entryNumber = await _numberService.GenerateJournalEntryNumberAsync(branchId, cancellationToken);

        var entry = new JournalEntry
        {
            EntryNumber  = entryNumber,
            EntryDate    = entryDate,
            FiscalYearId = fiscalYearId,
            BranchId     = branchId,
            Description  = description ?? $"{entryType} - {referenceDocNumber}",
            ReferenceId  = referenceId,
            CreatedAt    = DateTime.UtcNow,
        };

        var details = new List<JournalEntryDetail>();

        if (debitAccountId.HasValue)
        {
            details.Add(new JournalEntryDetail
            {
                JournalEntryId = entry.Oid,
                AccountId      = debitAccountId.Value,
                Description    = description ?? $"{entryType} - {referenceDocNumber}",
                Debit          = amount,
                Credit         = 0,
                CreatedAt      = DateTime.UtcNow,
            });
        }

        if (creditAccountId.HasValue)
        {
            details.Add(new JournalEntryDetail
            {
                JournalEntryId = entry.Oid,
                AccountId      = creditAccountId.Value,
                Description    = description ?? $"{entryType} - {referenceDocNumber}",
                Debit          = 0,
                Credit         = amount,
                CreatedAt      = DateTime.UtcNow,
            });
        }

        entry.TotalDebit  = details.Sum(d => d.Debit);
        entry.TotalCredit = details.Sum(d => d.Credit);

        await _journalRepo.InsertMasterDetailAsync(entry, details, cancellationToken);

        return entry;
    }
}
