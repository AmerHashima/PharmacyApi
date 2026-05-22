using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Services;

/// <summary>
/// ═══════════════════════════════════════════════════════════════════════════
/// PRODUCTION-GRADE ERP ACCOUNTING ENGINE
/// ═══════════════════════════════════════════════════════════════════════════
/// 
/// Creates balanced JournalEntry records for sales and return invoices.
/// SAP/Odoo-level sophistication with full double-entry compliance.
/// 
/// ╔═══════════════════════════════════════════════════════════════════════╗
/// ║ SALES INVOICE POSTING — MIXED VAT SUPPORT                             ║
/// ╚═══════════════════════════════════════════════════════════════════════╝
/// 
/// Section A — Sales Revenue (by VAT category):
///   DR  Customer/Receivable            = TotalAmount (gross)
///   DR  Discount Allowed (invoice-lvl) = InvoiceDiscountAmount (if > 0)
///   CR  Sales Revenue (taxable)        = Σ NetPrice (taxable items)
///   CR  Zero-Rated Sales               = Σ NetPrice (zero-rated items)
///   CR  Exempt Sales                   = Σ NetPrice (exempt items)
///   CR  VAT Output                     = Σ TaxAmount (taxable items ONLY)
/// 
/// Section B — Payment Settlement (per payment method):
///   DR  Cash Account                   = Payment.Amount (if CASH)
///   DR  Bank Account                   = Payment.Amount (if BANK)
///   CR  Customer/Receivable            = Σ Payments.Amount (clears receivable)
///   
///   Partial payment example:
///     Invoice Total = 1,000
///     Payment 1: CASH 500
///     Payment 2: BANK 300
///     Remaining: CREDIT 200 (stays in receivable)
/// 
/// Section C — Cost of Goods Sold (item-level):
///   FOR EACH item (non-free):
///     DR  COGS                         = CostPrice × Quantity
///     CR  Inventory                    = CostPrice × Quantity
/// 
/// ╔═══════════════════════════════════════════════════════════════════════╗
/// ║ RETURN INVOICE POSTING — CATEGORY-AWARE REVERSALS                     ║
/// ╚═══════════════════════════════════════════════════════════════════════╝
/// 
/// Section A — Sales Reversal (by VAT category):
///   DR  Sales Revenue (taxable)        = Σ NetPrice (taxable items)
///   DR  Zero-Rated Sales               = Σ NetPrice (zero-rated items)
///   DR  Exempt Sales                   = Σ NetPrice (exempt items)
///   DR  VAT Output                     = Σ TaxAmount (taxable items ONLY)
///   CR  Customer/Receivable            = TotalAmount (or Cash/Bank if refunded)
/// 
/// Section B — COGS Reversal (item-level):
///   FOR EACH returned item:
///     DR  Inventory                    = CostPrice × Quantity
///     CR  COGS                         = CostPrice × Quantity
/// 
/// ═══════════════════════════════════════════════════════════════════════════
/// ACCOUNT RESOLUTION RULES
/// ═══════════════════════════════════════════════════════════════════════════
/// 
/// Sales Revenue:
///   Taxable items    → AccountingSettings.SalesAccountId
///   Zero-rated items → AccountingSettings.ZeroRatedSalesAccountId
///   Exempt items     → AccountingSettings.ExemptSalesAccountId
/// 
/// VAT:
///   Taxable items    → AccountingSettings.VatOutputAccountId
///   Zero/Exempt      → NO VAT ENTRY
/// 
/// Discounts:
///   Invoice-level    → AccountingSettings.SalesDiscountAccountId
///   Line-level       → Reduces NetPrice (no separate entry)
/// 
/// Payment Methods:
///   CASH             → AccountingSettings.CashAccountId
///   BANK             → PaymentMethodDetail.BankAccountId ?? AccountingSettings.BankAccountId
///   CREDIT           → Customer.ChildAccountId ?? AccountingSettings.ReceivableAccountId
/// 
/// COGS/Inventory:
///   All items        → AccountingSettings.CogsAccountId / InventoryAccountId
/// 
/// ═══════════════════════════════════════════════════════════════════════════
/// </summary>
public sealed class JournalPostingService : IJournalPostingService
{
    private readonly IJournalEntryRepository _journalRepo;
    private readonly IVoucherNumberService _numberService;
    private readonly IAccountingSettingsRepository _settingsRepo;
    private readonly IFiscalYearRepository _fiscalYearRepo;
    private readonly ICustomerRepository _customerRepo;
    private readonly IStakeholderRepository _stakeholderRepo;
    private readonly ISalesInvoiceRepository _invoiceRepo;
    private readonly IReturnInvoiceRepository _returnInvoiceRepo;
    private readonly PharmacyDbContext _context;

    public const string RefTypeSalesInvoice  = "SALES_INV";
    public const string RefTypeReturnInvoice = "RETURN_INV";
    public const string TypeSalesEntry       = "SI";
    public const string TypePaymentEntry     = "SP";
    public const string TypeCogsEntry        = "COGS";
    public const string TypeReturnEntry      = "RI";

    public JournalPostingService(
        IJournalEntryRepository journalRepo,
        IVoucherNumberService numberService,
        IAccountingSettingsRepository settingsRepo,
        IFiscalYearRepository fiscalYearRepo,
        ICustomerRepository customerRepo,
        IStakeholderRepository stakeholderRepo,
        ISalesInvoiceRepository invoiceRepo,
        IReturnInvoiceRepository returnInvoiceRepo,
        PharmacyDbContext context)
    {
        _journalRepo       = journalRepo;
        _numberService     = numberService;
        _settingsRepo      = settingsRepo;
        _fiscalYearRepo    = fiscalYearRepo;
        _customerRepo      = customerRepo;
        _stakeholderRepo   = stakeholderRepo;
        _invoiceRepo       = invoiceRepo;
        _returnInvoiceRepo = returnInvoiceRepo;
        _context           = context;
    }

    // ═════════════════════════════════════════════════════════════════════════
    // SALES INVOICE POSTING — PRODUCTION-GRADE
    // ═════════════════════════════════════════════════════════════════════════
    public async Task<SalesInvoicePostingResult> PostSalesInvoiceAsync(
        SalesInvoicePostingRequest req,
        CancellationToken ct = default)
    {
        // ── 1. Duplicate-posting guard ────────────────────────────────────
        var invoice = await _invoiceRepo.GetByIdAsync(req.InvoiceOid, ct)
            ?? throw new KeyNotFoundException($"SalesInvoice '{req.InvoiceOid}' not found.");

        if (invoice.JournalEntryId.HasValue)
            throw new InvalidOperationException(
                $"SalesInvoice '{req.InvoiceNumber}' already posted (JE={invoice.JournalEntryId}).");

        // ── 2. FiscalYear closed guard ────────────────────────────────────
        if (req.FiscalYearId.HasValue)
        {
            var fy = await _fiscalYearRepo.GetByIdAsync(req.FiscalYearId.Value, ct);
            if (fy?.IsClosed == true)
                throw new InvalidOperationException(
                    $"Cannot post to closed fiscal year (ID={req.FiscalYearId}).");
        }

        // ── 3. Resolve AccountingSettings ─────────────────────────────────
        var settings = await _settingsRepo.GetByBranchAsync(req.BranchId, ct)
            ?? throw new InvalidOperationException(
                $"AccountingSettings not configured for branch '{req.BranchId}'.");

        // ── 4. Resolve customer account ───────────────────────────────────
        Guid? customerAccountId = await ResolveCustomerAccountAsync(req.CustomerId, ct)
                                  ?? settings.ReceivableAccountId;

        // ── 5. Group items by VAT category ────────────────────────────────
        var taxableItems   = req.Items.Where(i => i.VatCategory == VatCategory.Taxable).ToList();
        var zeroRatedItems = req.Items.Where(i => i.VatCategory == VatCategory.ZeroRated).ToList();
        var exemptItems    = req.Items.Where(i => i.VatCategory == VatCategory.Exempt).ToList();

        var taxableNet   = taxableItems.Sum(i => i.NetPrice);
        var taxableVat   = taxableItems.Sum(i => i.TaxAmount);
        var zeroRatedNet = zeroRatedItems.Sum(i => i.NetPrice);
        var exemptNet    = exemptItems.Sum(i => i.NetPrice);

        var totalPayments = req.Payments.Sum(p => p.Amount);
        var remainingReceivable = req.TotalAmount - totalPayments;

        // ── 6. Create JournalEntry master ─────────────────────────────────
        var entryNumber = await _numberService.GenerateJournalEntryNumberAsync(req.BranchId, ct);
        var entry = new JournalEntry
        {
            EntryNumber  = entryNumber,
            EntryDate    = req.InvoiceDate,
            FiscalYearId = req.FiscalYearId,
            BranchId     = req.BranchId,
            Description  = $"{TypeSalesEntry} - {req.InvoiceNumber}",
            ReferenceId  = req.InvoiceOid,
            CreatedAt    = DateTime.UtcNow,
        };

        var details = new List<JournalEntryDetail>();
        int seq = 1;

        // ══════════════════════════════════════════════════════════════════
        // SECTION A — SALES REVENUE (Mixed VAT Support)
        // ══════════════════════════════════════════════════════════════════

        // A1. DR Customer/Receivable = TotalAmount (always create to establish debt)
        // This receivable will be cleared in Section B by payments
        if (customerAccountId.HasValue)
            details.Add(Detail(entry.Oid, customerAccountId.Value,
                debit: req.TotalAmount, credit: 0,
                $"Sale - {req.InvoiceNumber}",
                $"مبيعات - فاتورة {req.InvoiceNumber}", seq++));

        // A2. DR Discount Allowed = InvoiceDiscountAmount (header-level discount)
        if (req.InvoiceDiscountAmount > 0 && settings.SalesDiscountAccountId.HasValue)
            details.Add(Detail(entry.Oid, settings.SalesDiscountAccountId.Value,
                debit: req.InvoiceDiscountAmount, credit: 0,
                $"Discount - {req.InvoiceNumber}",
                $"خصم ممنوح - فاتورة {req.InvoiceNumber}", seq++));

        // A3. CR Sales Revenue (Taxable) = Σ NetPrice (taxable items)
        if (taxableNet > 0 && settings.SalesAccountId.HasValue)
            details.Add(Detail(entry.Oid, settings.SalesAccountId.Value,
                debit: 0, credit: taxableNet,
                $"Taxable Sales - {req.InvoiceNumber}",
                $"مبيعات خاضعة للضريبة - فاتورة {req.InvoiceNumber}", seq++));

        // A4. CR Zero-Rated Sales = Σ NetPrice (zero-rated items)
        if (zeroRatedNet > 0 && settings.ZeroRatedSalesAccountId.HasValue)
            details.Add(Detail(entry.Oid, settings.ZeroRatedSalesAccountId.Value,
                debit: 0, credit: zeroRatedNet,
                $"Zero-Rated Sales - {req.InvoiceNumber}",
                $"مبيعات معفاة من الضريبة - فاتورة {req.InvoiceNumber}", seq++));

        // A5. CR Exempt Sales = Σ NetPrice (exempt items)
        if (exemptNet > 0 && settings.ExemptSalesAccountId.HasValue)
            details.Add(Detail(entry.Oid, settings.ExemptSalesAccountId.Value,
                debit: 0, credit: exemptNet,
                $"Exempt Sales - {req.InvoiceNumber}",
                $"مبيعات معفاة - فاتورة {req.InvoiceNumber}", seq++));

        // A6. CR VAT Output = Σ TaxAmount (taxable items ONLY)
        if (taxableVat > 0 && settings.VatOutputAccountId.HasValue)
            details.Add(Detail(entry.Oid, settings.VatOutputAccountId.Value,
                debit: 0, credit: taxableVat,
                $"VAT Output - {req.InvoiceNumber}",
                $"ضريبة مخرجات - فاتورة {req.InvoiceNumber}", seq++));

        // ══════════════════════════════════════════════════════════════════
        // SECTION B — PAYMENT SETTLEMENT (Partial/Mixed Payments)
        // ══════════════════════════════════════════════════════════════════

        foreach (var payment in req.Payments)
        {
            var methodCode = payment.MethodCode?.ToUpperInvariant();
            Guid? paymentAccountId = methodCode switch
            {
                "CASH" => settings.CashAccountId,
                "BANK" => payment.BankAccountId ?? settings.BankAccountId,
                "CREDIT" => null,  // CREDIT doesn't settle immediately
                _ => null
            };

            if (paymentAccountId.HasValue && payment.Amount > 0)
            {
                // DR Cash/Bank = Payment.Amount
                details.Add(Detail(entry.Oid, paymentAccountId.Value,
                    debit: payment.Amount, credit: 0,
                    $"Payment ({methodCode}) - {req.InvoiceNumber}",
                    $"تحصيل ({methodCode}) - فاتورة {req.InvoiceNumber}", seq++));
            }
        }

        // If any payments settled, CR Customer/Receivable to clear
        if (totalPayments > 0 && customerAccountId.HasValue)
            details.Add(Detail(entry.Oid, customerAccountId.Value,
                debit: 0, credit: totalPayments,
                $"Payment Settlement - {req.InvoiceNumber}",
                $"تسوية ذمم - فاتورة {req.InvoiceNumber}", seq++));

        // ══════════════════════════════════════════════════════════════════
        // SECTION C — COST OF GOODS SOLD (Item-Level for Audit Trail)
        // ══════════════════════════════════════════════════════════════════

        foreach (var item in req.Items.Where(i => !i.IsFreeItem))
        {
            var cogsAmount = item.CostPrice * item.Quantity;
            if (cogsAmount <= 0) continue;

            // DR COGS
            if (settings.CogsAccountId.HasValue)
                details.Add(Detail(entry.Oid, settings.CogsAccountId.Value,
                    debit: cogsAmount, credit: 0,
                    $"COGS - {item.ProductName} (Line {item.LineNumber})",
                    $"تكلفة البضاعة - {item.ProductName} (سطر {item.LineNumber})", seq++));

            // CR Inventory
            if (settings.InventoryAccountId.HasValue)
                details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                    debit: 0, credit: cogsAmount,
                    $"Inventory - {item.ProductName} (Line {item.LineNumber})",
                    $"مخزون - {item.ProductName} (سطر {item.LineNumber})", seq++));
        }

        // ── 7. Validate balance ───────────────────────────────────────────
        entry.TotalDebit  = details.Sum(d => d.Debit);
        entry.TotalCredit = details.Sum(d => d.Credit);

        if (Math.Abs(entry.TotalDebit - entry.TotalCredit) > 0.01m)
            throw new InvalidOperationException(
                $"Unbalanced journal entry: DR={entry.TotalDebit:F2}, CR={entry.TotalCredit:F2}");

        // ── 8. Persist in atomic transaction ──────────────────────────────
        var strategy = _context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                await _journalRepo.InsertMasterDetailAsync(entry, details, ct);

                invoice.JournalEntryId = entry.Oid;
                await _invoiceRepo.UpdateAsync(invoice, ct);

                await tx.CommitAsync(ct);
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        });

        return new SalesInvoicePostingResult(entry);
    }

    // ═════════════════════════════════════════════════════════════════════════
    // RETURN INVOICE POSTING — CATEGORY-AWARE REVERSALS
    // ═════════════════════════════════════════════════════════════════════════
    public async Task<JournalEntry> PostReturnInvoiceAsync(
        ReturnInvoicePostingRequest req,
        CancellationToken ct = default)
    {
        // ── 1. FiscalYear closed guard ────────────────────────────────────
        if (req.FiscalYearId.HasValue)
        {
            var fy = await _fiscalYearRepo.GetByIdAsync(req.FiscalYearId.Value, ct);
            if (fy?.IsClosed == true)
                throw new InvalidOperationException(
                    $"Cannot post to closed fiscal year (ID={req.FiscalYearId}).");
        }

        // ── 2. Resolve AccountingSettings ─────────────────────────────────
        var settings = await _settingsRepo.GetByBranchAsync(req.BranchId, ct)
            ?? throw new InvalidOperationException(
                $"AccountingSettings not configured for branch '{req.BranchId}'.");

        // ── 3. Resolve customer account ───────────────────────────────────
        Guid? customerAccountId = await ResolveCustomerAccountAsync(req.CustomerId, ct)
                                  ?? settings.ReceivableAccountId;

        // ── 4. Group items by VAT category ────────────────────────────────
        var taxableItems   = req.Items.Where(i => i.VatCategory == VatCategory.Taxable).ToList();
        var zeroRatedItems = req.Items.Where(i => i.VatCategory == VatCategory.ZeroRated).ToList();
        var exemptItems    = req.Items.Where(i => i.VatCategory == VatCategory.Exempt).ToList();

        var taxableNet   = taxableItems.Sum(i => i.NetPrice);
        var taxableVat   = taxableItems.Sum(i => i.TaxAmount);
        var zeroRatedNet = zeroRatedItems.Sum(i => i.NetPrice);
        var exemptNet    = exemptItems.Sum(i => i.NetPrice);

        var totalRefunds = req.RefundMethods.Sum(r => r.Amount);

        // ── 5. Create JournalEntry master ─────────────────────────────────
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
        int seq = 1;

        // ══════════════════════════════════════════════════════════════════
        // SECTION A — SALES REVERSAL (by VAT Category)
        // ══════════════════════════════════════════════════════════════════

        // A1. DR Sales Revenue (Taxable) — reversal
        if (taxableNet > 0 && settings.SalesAccountId.HasValue)
            details.Add(Detail(entry.Oid, settings.SalesAccountId.Value,
                debit: taxableNet, credit: 0,
                $"Return - Taxable Sales - {req.ReturnNumber}",
                $"مرتجع مبيعات خاضعة - مرتجع {req.ReturnNumber}", seq++));

        // A2. DR Zero-Rated Sales — reversal
        if (zeroRatedNet > 0 && settings.ZeroRatedSalesAccountId.HasValue)
            details.Add(Detail(entry.Oid, settings.ZeroRatedSalesAccountId.Value,
                debit: zeroRatedNet, credit: 0,
                $"Return - Zero-Rated Sales - {req.ReturnNumber}",
                $"مرتجع مبيعات معفاة - مرتجع {req.ReturnNumber}", seq++));

        // A3. DR Exempt Sales — reversal
        if (exemptNet > 0 && settings.ExemptSalesAccountId.HasValue)
            details.Add(Detail(entry.Oid, settings.ExemptSalesAccountId.Value,
                debit: exemptNet, credit: 0,
                $"Return - Exempt Sales - {req.ReturnNumber}",
                $"مرتجع مبيعات معفاة - مرتجع {req.ReturnNumber}", seq++));

        // A4. DR VAT Output — reversal (taxable items ONLY)
        if (taxableVat > 0 && settings.VatOutputAccountId.HasValue)
            details.Add(Detail(entry.Oid, settings.VatOutputAccountId.Value,
                debit: taxableVat, credit: 0,
                $"Return - VAT Reversal - {req.ReturnNumber}",
                $"مرتجع ضريبة مخرجات - مرتجع {req.ReturnNumber}", seq++));

        // ══════════════════════════════════════════════════════════════════
        // SECTION B — REFUND SETTLEMENT
        // ══════════════════════════════════════════════════════════════════

        foreach (var refund in req.RefundMethods)
        {
            var methodCode = refund.MethodCode?.ToUpperInvariant();
            Guid? refundAccountId = methodCode switch
            {
                "CASH" => settings.CashAccountId,
                "BANK" => refund.BankAccountId ?? settings.BankAccountId,
                "CREDIT" => customerAccountId,  // Adjust receivable
                _ => customerAccountId
            };

            if (refundAccountId.HasValue && refund.Amount > 0)
            {
                // CR Cash/Bank/Receivable = Refund.Amount
                details.Add(Detail(entry.Oid, refundAccountId.Value,
                    debit: 0, credit: refund.Amount,
                    $"Refund ({methodCode}) - {req.ReturnNumber}",
                    $"استرداد ({methodCode}) - مرتجع {req.ReturnNumber}", seq++));
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // SECTION C — COGS REVERSAL (Item-Level)
        // ══════════════════════════════════════════════════════════════════

        foreach (var item in req.Items.Where(i => !i.IsFreeItem))
        {
            var cogsAmount = item.CostPrice * item.Quantity;
            if (cogsAmount <= 0) continue;

            // DR Inventory (restore)
            if (settings.InventoryAccountId.HasValue)
                details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                    debit: cogsAmount, credit: 0,
                    $"Inventory Restore - {item.ProductName} (Line {item.LineNumber})",
                    $"استرجاع مخزون - {item.ProductName} (سطر {item.LineNumber})", seq++));

            // CR COGS (reversal)
            if (settings.CogsAccountId.HasValue)
                details.Add(Detail(entry.Oid, settings.CogsAccountId.Value,
                    debit: 0, credit: cogsAmount,
                    $"COGS Reversal - {item.ProductName} (Line {item.LineNumber})",
                    $"عكس تكلفة البضاعة - {item.ProductName} (سطر {item.LineNumber})", seq++));
        }

        // ── 6. Validate balance ───────────────────────────────────────────
        entry.TotalDebit  = details.Sum(d => d.Debit);
        entry.TotalCredit = details.Sum(d => d.Credit);

        if (Math.Abs(entry.TotalDebit - entry.TotalCredit) > 0.01m)
            throw new InvalidOperationException(
                $"Unbalanced return entry: DR={entry.TotalDebit:F2}, CR={entry.TotalCredit:F2}");

        // ── 7. Persist in atomic transaction ──────────────────────────────
        var strategy = _context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                await _journalRepo.InsertMasterDetailAsync(entry, details, ct);

                var returnInvoice = await _returnInvoiceRepo.GetByIdAsync(req.ReturnInvoiceOid, ct);
                if (returnInvoice != null)
                {
                    returnInvoice.JournalEntryId = entry.Oid;
                    await _returnInvoiceRepo.UpdateAsync(returnInvoice, ct);
                }

                await tx.CommitAsync(ct);
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        });

        return entry;
    }

    // ═════════════════════════════════════════════════════════════════════════
    // STOCK TRANSACTION POSTING
    // ═════════════════════════════════════════════════════════════════════════
    //
    // Journal rules:
    //   IN         → DR Inventory              CR Purchase (Accounts Payable)
    //   OUT        → DR COGS / InventoryLoss   CR Inventory
    //   TRANSFER   → DR Inventory (To-branch)  CR Inventory (From-branch)  [StockTransfer clearing]
    //   RETURN     → DR Accounts Payable       CR Inventory                 [purchase return to supplier]
    //   ADJUSTMENT → DR/CR Inventory           CR/DR InventoryAdjustment
    //   EXPIRED    → DR ExpiredItems           CR Inventory
    //   DAMAGED    → DR DamagedInventory       CR Inventory
    // ═════════════════════════════════════════════════════════════════════════
    public async Task<JournalEntry> PostStockTransactionAsync(
        StockTransactionPostingRequest req,
        CancellationToken ct = default)
    {
        // ── 1. FiscalYear closed guard ────────────────────────────────────
        if (req.FiscalYearId.HasValue)
        {
            var fy = await _fiscalYearRepo.GetByIdAsync(req.FiscalYearId.Value, ct);
            if (fy?.IsClosed == true)
                throw new InvalidOperationException(
                    $"Cannot post to closed fiscal year (ID={req.FiscalYearId}).");
        }

        // ── 2. Resolve AccountingSettings ─────────────────────────────────
        var settings = await _settingsRepo.GetByBranchAsync(req.BranchId, ct)
            ?? throw new InvalidOperationException(
                $"AccountingSettings not configured for branch '{req.BranchId}'.");

        // ── 3. Compute total value ────────────────────────────────────────
        var totalValue = req.Items.Sum(i => i.TotalCost);
        var typeCode   = req.TypeCode.ToUpperInvariant();

        // ── 3a. Resolve supplier's ledger account (for IN / RETURN) ───────
        Guid? supplierAccountId = await ResolveSupplierAccountAsync(req.SupplierId, ct)
            ?? settings.SupplierPayableAccountId
            ?? settings.PurchaseAccountId;

        // ── 4. Create JournalEntry master ─────────────────────────────────
        var entryNumber = await _numberService.GenerateJournalEntryNumberAsync(req.BranchId, ct);
        var entry = new JournalEntry
        {
            EntryNumber  = entryNumber,
            EntryDate    = req.TransactionDate,
            FiscalYearId = req.FiscalYearId,
            BranchId     = req.BranchId,
            Description  = $"{typeCode} - {req.ReferenceNumber}",
            ReferenceId  = req.TransactionOid,
            CreatedAt    = DateTime.UtcNow,
        };

        var details = new List<JournalEntryDetail>();
        int seq = 1;

        switch (typeCode)
        {
            // ─────────────────────────────────────────────────────────────
            // IN — Stock received from supplier
            //   DR Inventory        = totalValue
            //   CR Supplier Payable = totalValue
            // ─────────────────────────────────────────────────────────────
            case "IN":
                if (settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: totalValue, credit: 0,
                        $"Stock IN - {req.ReferenceNumber}",
                        $"بضاعة واردة - {req.ReferenceNumber}", seq++));

                if (supplierAccountId.HasValue)
                    details.Add(Detail(entry.Oid, supplierAccountId.Value,
                        debit: 0, credit: totalValue,
                        $"Supplier Payable - {req.ReferenceNumber}",
                        $"دائن مورد - {req.ReferenceNumber}", seq++));
                break;

            // ─────────────────────────────────────────────────────────────
            // OUT — Stock issued (manual write-off)
            //   DR COGS      = totalValue
            //   CR Inventory = totalValue
            // ─────────────────────────────────────────────────────────────
            case "OUT":
                if (settings.CogsAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.CogsAccountId.Value,
                        debit: totalValue, credit: 0,
                        $"Stock OUT - {req.ReferenceNumber}",
                        $"بضاعة صادرة - {req.ReferenceNumber}", seq++));

                if (settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: 0, credit: totalValue,
                        $"Inventory OUT - {req.ReferenceNumber}",
                        $"مخزون صادر - {req.ReferenceNumber}", seq++));
                break;

            // ─────────────────────────────────────────────────────────────
            // TRANSFER — Move stock between branches
            //   DR StockTransfer (transit clearing) = totalValue
            //   CR Inventory (from-branch)           = totalValue
            // ─────────────────────────────────────────────────────────────
            case "TRANSFER":
                var transitAccountId = settings.StockTransferAccountId ?? settings.InventoryAccountId;

                if (transitAccountId.HasValue)
                    details.Add(Detail(entry.Oid, transitAccountId.Value,
                        debit: totalValue, credit: 0,
                        $"Transfer OUT - {req.ReferenceNumber}",
                        $"تحويل مخزون - {req.ReferenceNumber}", seq++));

                if (settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: 0, credit: totalValue,
                        $"Transfer Source - {req.ReferenceNumber}",
                        $"مصدر التحويل - {req.ReferenceNumber}", seq++));
                break;

            // ─────────────────────────────────────────────────────────────
            // RETURN — Purchase return to supplier
            //   DR Supplier Payable = totalValue
            //   CR Inventory        = totalValue
            // ─────────────────────────────────────────────────────────────
            case "RETURN":
                if (supplierAccountId.HasValue)
                    details.Add(Detail(entry.Oid, supplierAccountId.Value,
                        debit: totalValue, credit: 0,
                        $"Purchase Return - {req.ReferenceNumber}",
                        $"مدين مورد - {req.ReferenceNumber}", seq++));

                if (settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: 0, credit: totalValue,
                        $"Return to Supplier - {req.ReferenceNumber}",
                        $"إرجاع للمورد - {req.ReferenceNumber}", seq++));
                break;

            // ─────────────────────────────────────────────────────────────
            // ADJUSTMENT — Inventory count correction
            //   Surplus (totalValue > 0): DR Inventory  CR AdjustmentAccount
            //   Shortage (totalValue < 0): DR AdjustmentAccount  CR Inventory
            // ─────────────────────────────────────────────────────────────
            case "ADJUSTMENT":
                var adjAccountId = settings.InventoryAdjustmentAccountId ?? settings.InventoryLossAccountId;
                if (adjAccountId.HasValue && settings.InventoryAccountId.HasValue)
                {
                    var absTotal  = Math.Abs(totalValue);
                    bool isSurplus = totalValue >= 0;

                    details.Add(Detail(entry.Oid,
                        isSurplus ? settings.InventoryAccountId.Value : adjAccountId.Value,
                        debit: absTotal, credit: 0,
                        $"Inventory Adjustment - {req.ReferenceNumber}",
                        $"تسوية مخزون - {req.ReferenceNumber}", seq++));

                    details.Add(Detail(entry.Oid,
                        isSurplus ? adjAccountId.Value : settings.InventoryAccountId.Value,
                        debit: 0, credit: absTotal,
                        $"Inventory Adjustment - {req.ReferenceNumber}",
                        $"تسوية مخزون - {req.ReferenceNumber}", seq++));
                }
                break;

            // ─────────────────────────────────────────────────────────────
            // EXPIRED — Write off expired pharmaceuticals
            //   DR ExpiredItems = totalValue
            //   CR Inventory    = totalValue
            // ─────────────────────────────────────────────────────────────
            case "EXPIRED":
                var expiredAccountId = settings.ExpiredItemsAccountId ?? settings.InventoryLossAccountId;

                if (expiredAccountId.HasValue)
                    details.Add(Detail(entry.Oid, expiredAccountId.Value,
                        debit: totalValue, credit: 0,
                        $"Expired Write-off - {req.ReferenceNumber}",
                        $"شطب منتهي الصلاحية - {req.ReferenceNumber}", seq++));

                if (settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: 0, credit: totalValue,
                        $"Expired Inventory - {req.ReferenceNumber}",
                        $"مخزون منتهي الصلاحية - {req.ReferenceNumber}", seq++));
                break;

            // ─────────────────────────────────────────────────────────────
            // DAMAGED — Write off damaged stock
            //   DR DamagedInventory = totalValue
            //   CR Inventory        = totalValue
            // ─────────────────────────────────────────────────────────────
            case "DAMAGED":
                var damagedAccountId = settings.DamagedInventoryAccountId ?? settings.InventoryLossAccountId;

                if (damagedAccountId.HasValue)
                    details.Add(Detail(entry.Oid, damagedAccountId.Value,
                        debit: totalValue, credit: 0,
                        $"Damaged Write-off - {req.ReferenceNumber}",
                        $"شطب بضاعة تالفة - {req.ReferenceNumber}", seq++));

                if (settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: 0, credit: totalValue,
                        $"Damaged Inventory - {req.ReferenceNumber}",
                        $"مخزون تالف - {req.ReferenceNumber}", seq++));
                break;

            default:
                throw new InvalidOperationException($"Unknown transaction type '{req.TypeCode}' for journal posting.");
        }

        // ── 5. Validate balance ───────────────────────────────────────────
        entry.TotalDebit  = details.Sum(d => d.Debit);
        entry.TotalCredit = details.Sum(d => d.Credit);

        if (Math.Abs(entry.TotalDebit - entry.TotalCredit) > 0.01m)
            throw new InvalidOperationException(
                $"Unbalanced stock transaction entry: DR={entry.TotalDebit:F2}, CR={entry.TotalCredit:F2}");

        // ── 6. Persist in atomic transaction ──────────────────────────────
        var strategy = _context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
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
        });

        return entry;
    }

    private async Task<Guid?> ResolveCustomerAccountAsync(Guid? customerId, CancellationToken ct)
    {
        if (!customerId.HasValue) return null;
        var customer = await _customerRepo.GetByIdAsync(customerId.Value, ct);
        return customer?.ChildAccountId;
    }

    private async Task<Guid?> ResolveSupplierAccountAsync(Guid? supplierId, CancellationToken ct)
    {
        if (!supplierId.HasValue) return null;
        var supplier = await _stakeholderRepo.GetByIdAsync(supplierId.Value, ct);
        return supplier?.ChildAccountId;
    }

    private static JournalEntryDetail Detail(
        Guid entryId, Guid accountId, decimal debit, decimal credit,
        string description, string descriptionAr, int lineNumber = 0)
        => new()
        {
            JournalEntryId = entryId,
            AccountId      = accountId,
            Description    = description,
            DescriptionAr  = descriptionAr,
            Debit          = debit,
            Credit         = credit,
            LineNumber     = lineNumber,
            CreatedAt      = DateTime.UtcNow,
        };
}
