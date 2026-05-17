using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Services;

/// <summary>
/// Creates balanced JournalEntry records for sales and return invoices.
///
/// Sales invoice posting generates TWO journal entries in one DB transaction:
///
///   Entry 1 — Sales:
///     DR  Cash / Bank / Receivable  =  TotalAmount
///     DR  Discount Allowed          =  DiscountAmount  (if &gt; 0)
///     CR  Sales Revenue             =  SubTotal
///     CR  VAT Payable               =  TaxAmount       (if &gt; 0)
///
///   Entry 2 — COGS:
///     DR  Cost of Goods Sold        =  Σ(CostPrice × Qty)
///     CR  Inventory                 =  Σ(CostPrice × Qty)
///
/// Account IDs are resolved from AccountingSettings per branch.
/// Payment method determines the debit account for Entry 1:
///   "CASH"   → AccountingSettings.CashAccountId
///   "BANK"   → AccountingSettings.BankAccountId
///   "CREDIT" → Customer.ChildAccountId  (accounts receivable)
///   (null)   → AccountingSettings.ReceivableAccountId  (fallback)
/// </summary>
public sealed class JournalPostingService : IJournalPostingService
{
    private readonly IJournalEntryRepository _journalRepo;
    private readonly IVoucherNumberService _numberService;
    private readonly IAccountingSettingsRepository _settingsRepo;
    private readonly IFiscalYearRepository _fiscalYearRepo;
    private readonly ICustomerRepository _customerRepo;
    private readonly ISalesInvoiceRepository _invoiceRepo;
    private readonly PharmacyDbContext _context;

    // ValueCode constants — must match AppLookupDetail rows seeded under LookupCode="JOURNAL_REF_TYPE"
    public const string RefTypeSalesInvoice  = "SALES_INV";
    public const string RefTypeReturnInvoice = "RETURN_INV";
    public const string TypeSalesEntry       = "SI";
    public const string TypeCogsEntry        = "COGS";
    public const string TypeReturnEntry      = "RI";

    public JournalPostingService(
        IJournalEntryRepository journalRepo,
        IVoucherNumberService numberService,
        IAccountingSettingsRepository settingsRepo,
        IFiscalYearRepository fiscalYearRepo,
        ICustomerRepository customerRepo,
        ISalesInvoiceRepository invoiceRepo,
        PharmacyDbContext context)
    {
        _journalRepo  = journalRepo;
        _numberService = numberService;
        _settingsRepo  = settingsRepo;
        _fiscalYearRepo = fiscalYearRepo;
        _customerRepo  = customerRepo;
        _invoiceRepo   = invoiceRepo;
        _context       = context;
    }

    // ─────────────────────────────────────────────────────────────────────
    public async Task<SalesInvoicePostingResult> PostSalesInvoiceAsync(
        SalesInvoicePostingRequest req,
        CancellationToken ct = default)
    {
        // ── 1. Duplicate-posting guard ────────────────────────────────────
        var invoice = await _invoiceRepo.GetByIdAsync(req.InvoiceOid, ct)
            ?? throw new KeyNotFoundException($"SalesInvoice '{req.InvoiceOid}' not found.");

        if (invoice.JournalEntryId.HasValue)
            throw new InvalidOperationException(
                $"SalesInvoice '{req.InvoiceNumber}' has already been posted (JE={invoice.JournalEntryId}).");

        // ── 2. FiscalYear closed guard ────────────────────────────────────
        if (req.FiscalYearId.HasValue)
        {
            var fy = await _fiscalYearRepo.GetByIdAsync(req.FiscalYearId.Value, ct);
            if (fy?.IsClosed == true)
                throw new InvalidOperationException(
                    $"Cannot post to a closed fiscal year (ID={req.FiscalYearId}).");
        }

        // ── 3. Resolve AccountingSettings ─────────────────────────────────
        var settings = await _settingsRepo.GetByBranchAsync(req.BranchId, ct)
            ?? throw new InvalidOperationException(
                $"AccountingSettings not configured for branch '{req.BranchId}'.");

        // ── 4. Resolve debit account (payment method routing) ────────────
        Guid? debitAccountId = req.PaymentMethodCode?.ToUpperInvariant() switch
        {
            "CASH"   => settings.CashAccountId,
            "BANK"   => settings.BankAccountId,
            "CREDIT" => await ResolveCustomerAccountAsync(req.CustomerId, ct),
            _        => settings.ReceivableAccountId
        };

        // ── 5. Build Entry 1 — Sales ──────────────────────────────────────
        var salesEntryNumber = await _numberService.GenerateJournalEntryNumberAsync(req.BranchId, ct);
        var salesEntry = new JournalEntry
        {
            EntryNumber  = salesEntryNumber,
            EntryDate    = req.InvoiceDate,
            FiscalYearId = req.FiscalYearId,
            BranchId     = req.BranchId,
            Description  = $"{TypeSalesEntry} - {req.InvoiceNumber}",
            ReferenceId  = req.InvoiceOid,
            CreatedAt    = DateTime.UtcNow,
        };

        var salesDetails = new List<JournalEntryDetail>();

        // DR  Cash / Bank / Customer
        if (debitAccountId.HasValue)
            salesDetails.Add(Detail(salesEntry.Oid, debitAccountId.Value,
                debit: req.TotalAmount, credit: 0, req.InvoiceNumber));

        // DR  Discount Allowed
        if (req.DiscountAmount > 0 && settings.DiscountAccountId.HasValue)
            salesDetails.Add(Detail(salesEntry.Oid, settings.DiscountAccountId.Value,
                debit: req.DiscountAmount, credit: 0, req.InvoiceNumber));

        // CR  Sales Revenue
        if (settings.SalesAccountId.HasValue)
            salesDetails.Add(Detail(salesEntry.Oid, settings.SalesAccountId.Value,
                debit: 0, credit: req.SubTotal, req.InvoiceNumber));

        // CR  VAT Payable
        if (req.TaxAmount > 0 && settings.VatAccountId.HasValue)
            salesDetails.Add(Detail(salesEntry.Oid, settings.VatAccountId.Value,
                debit: 0, credit: req.TaxAmount, req.InvoiceNumber));

        salesEntry.TotalDebit  = salesDetails.Sum(d => d.Debit);
        salesEntry.TotalCredit = salesDetails.Sum(d => d.Credit);

        // ── 6. Build Entry 2 — COGS ───────────────────────────────────────
        var cogsTotal = req.Items.Sum(i => i.CostPrice * i.Quantity);

        var cogsEntryNumber = await _numberService.GenerateJournalEntryNumberAsync(req.BranchId, ct);
        var cogsEntry = new JournalEntry
        {
            EntryNumber  = cogsEntryNumber,
            EntryDate    = req.InvoiceDate,
            FiscalYearId = req.FiscalYearId,
            BranchId     = req.BranchId,
            Description  = $"{TypeCogsEntry} - {req.InvoiceNumber}",
            ReferenceId  = req.InvoiceOid,
            CreatedAt    = DateTime.UtcNow,
        };

        var cogsDetails = new List<JournalEntryDetail>();

        if (cogsTotal > 0)
        {
            // DR  COGS
            if (settings.CogsAccountId.HasValue)
                cogsDetails.Add(Detail(cogsEntry.Oid, settings.CogsAccountId.Value,
                    debit: cogsTotal, credit: 0, req.InvoiceNumber));

            // CR  Inventory
            if (settings.InventoryAccountId.HasValue)
                cogsDetails.Add(Detail(cogsEntry.Oid, settings.InventoryAccountId.Value,
                    debit: 0, credit: cogsTotal, req.InvoiceNumber));
        }

        cogsEntry.TotalDebit  = cogsDetails.Sum(d => d.Debit);
        cogsEntry.TotalCredit = cogsDetails.Sum(d => d.Credit);

        // ── 7. Persist both entries + patch invoice — all in one transaction
        await using var tx = await _context.Database.BeginTransactionAsync(ct);
        try
        {
            await _journalRepo.InsertMasterDetailAsync(salesEntry, salesDetails, ct);
            await _journalRepo.InsertMasterDetailAsync(cogsEntry, cogsDetails, ct);

            invoice.JournalEntryId = salesEntry.Oid;
            await _invoiceRepo.UpdateAsync(invoice, ct);

            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }

        return new SalesInvoicePostingResult(salesEntry, cogsEntry);
    }

    // ─────────────────────────────────────────────────────────────────────
    public async Task<JournalEntry> PostReturnInvoiceAsync(
        ReturnInvoicePostingRequest req,
        CancellationToken ct = default)
    {
        // FiscalYear closed guard
        if (req.FiscalYearId.HasValue)
        {
            var fy = await _fiscalYearRepo.GetByIdAsync(req.FiscalYearId.Value, ct);
            if (fy?.IsClosed == true)
                throw new InvalidOperationException(
                    $"Cannot post to a closed fiscal year (ID={req.FiscalYearId}).");
        }

        var settings = await _settingsRepo.GetByBranchAsync(req.BranchId, ct)
            ?? throw new InvalidOperationException(
                $"AccountingSettings not configured for branch '{req.BranchId}'.");

        Guid? creditAccountId = req.PaymentMethodCode?.ToUpperInvariant() switch
        {
            "CASH"   => settings.CashAccountId,
            "BANK"   => settings.BankAccountId,
            "CREDIT" => await ResolveCustomerAccountAsync(req.CustomerId, ct),
            _        => settings.ReceivableAccountId
        };

        var entryNumber = await _numberService.GenerateJournalEntryNumberAsync(req.BranchId, ct);

        var entry = new JournalEntry
        {
            EntryNumber  = entryNumber,
            EntryDate    = req.ReturnDate,
            FiscalYearId = req.FiscalYearId,
            BranchId     = req.BranchId,
            Description  = $"{TypeReturnEntry} - {req.ReturnNumber}",
            ReferenceId  = req.ReturnInvoiceOid,
            CreatedAt    = DateTime.UtcNow,
        };

        var details = new List<JournalEntryDetail>();

        // DR  Sales Revenue (reversal)
        if (settings.SalesAccountId.HasValue)
            details.Add(Detail(entry.Oid, settings.SalesAccountId.Value,
                debit: req.SubTotal, credit: 0, req.ReturnNumber));

        // DR  VAT Payable (reversal)
        if (req.TaxAmount > 0 && settings.VatAccountId.HasValue)
            details.Add(Detail(entry.Oid, settings.VatAccountId.Value,
                debit: req.TaxAmount, credit: 0, req.ReturnNumber));

        // CR  Cash / Bank / Customer (refund)
        if (creditAccountId.HasValue)
            details.Add(Detail(entry.Oid, creditAccountId.Value,
                debit: 0, credit: req.TotalAmount, req.ReturnNumber));

        entry.TotalDebit  = details.Sum(d => d.Debit);
        entry.TotalCredit = details.Sum(d => d.Credit);

        await using var tx = await _context.Database.BeginTransactionAsync(ct);
        try
        {
            await _journalRepo.InsertMasterDetailAsync(entry, details, ct);
            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }

        return entry;
    }

    // ─────────────────────────────────────────────────────────────────────
    private async Task<Guid?> ResolveCustomerAccountAsync(Guid? customerId, CancellationToken ct)
    {
        if (!customerId.HasValue) return null;
        var customer = await _customerRepo.GetByIdAsync(customerId.Value, ct);
        return customer?.ChildAccountId;
    }

    private static JournalEntryDetail Detail(
        Guid entryId, Guid accountId, decimal debit, decimal credit, string refNumber)
        => new()
        {
            JournalEntryId = entryId,
            AccountId      = accountId,
            Description    = refNumber,
            Debit          = debit,
            Credit         = credit,
            CreatedAt      = DateTime.UtcNow,
        };
}