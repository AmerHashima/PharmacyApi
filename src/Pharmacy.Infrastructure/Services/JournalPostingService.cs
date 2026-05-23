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

        // ── 3. Compute amounts ────────────────────────────────────────────
        var typeCode   = req.TypeCode.ToUpperInvariant();

        // inventoryAmount = Σ all net costs regardless of VAT category
        // (taxable + zero-rated + exempt — all go into the Inventory debit)
        var inventoryAmount = req.TaxableNetCost + req.ZeroVatNetCost + req.ExemptNetCost;

        // grossTotal = inventory at cost + recoverable VAT on taxable items only
        var grossTotal = inventoryAmount + req.TaxableVatAmount;

        // For non-purchase types (OUT/TRANSFER/etc.) fall back to Items sum
        var totalValue = grossTotal > 0 ? grossTotal : req.Items.Sum(i => i.TotalCost);
        var netAmount  = inventoryAmount > 0 ? inventoryAmount : totalValue;

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
            // IN — Purchase invoice received from supplier
            //
            // Debit:
            //   DR Inventory  = TaxableNet + ZeroVatNet + ExemptNet  (ALL items at net cost)
            //   DR VAT Input  = TaxableVatAmount                     (taxable items ONLY)
            //
            // Credit:
            //   CR Cash/Bank        = PayedAmount            (if any immediate payment)
            //   CR Supplier Payable = GrossTotal - PayedAmount (unpaid remainder)
            //
            // Rules:
            //   • Zero-VAT and Exempt items are included in Inventory debit — no separate VAT line.
            //   • VAT Input line is posted ONLY when TaxableVatAmount > 0.
            //   • GrossTotal = (TaxableNet + ZeroVatNet + ExemptNet) + TaxableVatAmount.
            //   • Journal balances: DR(Inventory + VatInput) = CR(Cash + SupplierPayable).
            // ─────────────────────────────────────────────────────────────
            // ─────────────────────────────────────────────────────────────
            // IN — Purchase invoice received from supplier
            //
            // Entry 1 (this entry):
            //   DR Inventory (taxable net)     = TaxableNetCost
            //   DR Inventory (zero-VAT net)    = ZeroVatNetCost   → PurchaseWithoutVatAccount
            //   DR Inventory (exempt net)      = ExemptNetCost
            //   DR VAT Input                   = TaxableVatAmount  (taxable items ONLY)
            //   CR Supplier Payable            = GrossTotal        (always full amount)
            //
            // Entry 2 (payment settlement — posted separately if PayedAmount > 0):
            //   DR Supplier Payable = PayedAmount
            //   CR Cash             = PayedAmount
            // ─────────────────────────────────────────────────────────────
            case "IN":
                // DR Inventory (taxable items — net cost excl. VAT)
                if (req.TaxableNetCost > 0 && settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: req.TaxableNetCost, credit: 0,
                        $"Stock IN Taxable - {req.ReferenceNumber}",
                        $"بضاعة واردة خاضعة - {req.ReferenceNumber}", seq++));

                // DR Purchase Without VAT (zero-rated items — no VAT line will follow)
                if (req.ZeroVatNetCost > 0)
                {
                    var zeroVatAccountId = settings.PurchaseWithoutVatAccountId ?? settings.InventoryAccountId;
                    if (zeroVatAccountId.HasValue)
                        details.Add(Detail(entry.Oid, zeroVatAccountId.Value,
                            debit: req.ZeroVatNetCost, credit: 0,
                            $"Stock IN Zero-VAT - {req.ReferenceNumber}",
                            $"بضاعة واردة صفرية الضريبة - {req.ReferenceNumber}", seq++));
                }

                // DR Inventory (exempt items — no VAT line)
                if (req.ExemptNetCost > 0 && settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: req.ExemptNetCost, credit: 0,
                        $"Stock IN Exempt - {req.ReferenceNumber}",
                        $"بضاعة واردة معفاة - {req.ReferenceNumber}", seq++));

                // DR VAT Input — taxable items ONLY; zero-rated and exempt produce NO VAT line
                if (req.TaxableVatAmount > 0 && settings.VatInputAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.VatInputAccountId.Value,
                        debit: req.TaxableVatAmount, credit: 0,
                        $"VAT Input - {req.ReferenceNumber}",
                        $"ضريبة مدخلات - {req.ReferenceNumber}", seq++));

                // CR Cash — amount paid immediately at time of receipt
                if (req.PayedAmount > 0 && settings.CashAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.CashAccountId.Value,
                        debit: 0, credit: req.PayedAmount,
                        $"Cash Payment - {req.ReferenceNumber}",
                        $"دفع نقدي - {req.ReferenceNumber}", seq++));

                // CR Supplier Payable — unpaid remainder (GrossTotal - PayedAmount)
                var inRemainingPayable = grossTotal - req.PayedAmount;
                if (inRemainingPayable > 0 && supplierAccountId.HasValue)
                    details.Add(Detail(entry.Oid, supplierAccountId.Value,
                        debit: 0, credit: inRemainingPayable,
                        $"Supplier Payable - {req.ReferenceNumber}",
                        $"دائن المورد - {req.ReferenceNumber}", seq++));
                break;

            // ─────────────────────────────────────────────────────────────
            // OUT — Stock issued (manual write-off)
            //   DR COGS      = totalValue
            //   CR Inventory = totalValue
            // ─────────────────────────────────────────────────────────────
            case "OUT":
                if (settings.CogsAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.CogsAccountId.Value,
                        debit: netAmount, credit: 0,
                        $"Stock OUT - {req.ReferenceNumber}",
                        $"بضاعة صادرة - {req.ReferenceNumber}", seq++));

                if (settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: 0, credit: netAmount,
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
                        debit: netAmount, credit: 0,
                        $"Transfer OUT - {req.ReferenceNumber}",
                        $"تحويل مخزون - {req.ReferenceNumber}", seq++));

                if (settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: 0, credit: netAmount,
                        $"Transfer Source - {req.ReferenceNumber}",
                        $"مصدر التحويل - {req.ReferenceNumber}", seq++));
                break;

            // ─────────────────────────────────────────────────────────────
            // RETURN — Purchase return to supplier (exact reversal of IN)
            //
            // Debit:
            //   DR Supplier Payable = GrossTotal - PayedAmount  (reduce payable for unpaid portion)
            //   DR Cash             = PayedAmount               (cash refund for already-paid portion)
            //
            // Credit:
            //   CR VAT Input  = TaxableVatAmount  (reverse recoverable VAT — taxable items ONLY)
            //   CR Inventory  = TaxableNet + ZeroVatNet + ExemptNet
            //
            // Rules:
            //   • VAT Input reversal only if TaxableVatAmount > 0 (mirror of IN).
            //   • Journal balances: DR(SupplierPayable + Cash) = CR(VatInput + Inventory).
            // ─────────────────────────────────────────────────────────────
            // ─────────────────────────────────────────────────────────────
            // RETURN — Purchase return to supplier (exact reversal of IN)
            //
            // Entry 1 (this entry):
            //   DR Supplier Payable = GrossTotal       (always full amount)
            //   CR VAT Input        = TaxableVatAmount (taxable items ONLY)
            //   CR Inventory (taxable net)  = TaxableNetCost
            //   CR Inventory (zero-VAT net) = ZeroVatNetCost → PurchaseWithoutVatAccount
            //   CR Inventory (exempt net)   = ExemptNetCost
            //
            // Entry 2 (cash refund — posted separately if PayedAmount > 0):
            //   DR Cash             = PayedAmount
            //   CR Supplier Payable = PayedAmount
            // ─────────────────────────────────────────────────────────────
            case "RETURN":
                // DR Supplier Payable — unpaid remainder only
                var retRemainingPayable = grossTotal - req.PayedAmount;
                if (retRemainingPayable > 0 && supplierAccountId.HasValue)
                    details.Add(Detail(entry.Oid, supplierAccountId.Value,
                        debit: retRemainingPayable, credit: 0,
                        $"Purchase Return - {req.ReferenceNumber}",
                        $"مدين المورد - {req.ReferenceNumber}", seq++));

                // DR Cash — refund of amount already paid
                if (req.PayedAmount > 0 && settings.CashAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.CashAccountId.Value,
                        debit: req.PayedAmount, credit: 0,
                        $"Cash Refund - {req.ReferenceNumber}",
                        $"استرداد نقدي - {req.ReferenceNumber}", seq++));

                // CR VAT Input — reverse the recoverable tax (taxable items ONLY)
                if (req.TaxableVatAmount > 0 && settings.VatInputAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.VatInputAccountId.Value,
                        debit: 0, credit: req.TaxableVatAmount,
                        $"VAT Input Reversal - {req.ReferenceNumber}",
                        $"عكس ضريبة مدخلات - {req.ReferenceNumber}", seq++));

                // CR Inventory (taxable items)
                if (req.TaxableNetCost > 0 && settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: 0, credit: req.TaxableNetCost,
                        $"Return Taxable - {req.ReferenceNumber}",
                        $"إرجاع خاضع - {req.ReferenceNumber}", seq++));

                // CR Purchase Without VAT (zero-rated items)
                if (req.ZeroVatNetCost > 0)
                {
                    var zeroVatAccountId = settings.PurchaseWithoutVatAccountId ?? settings.InventoryAccountId;
                    if (zeroVatAccountId.HasValue)
                        details.Add(Detail(entry.Oid, zeroVatAccountId.Value,
                            debit: 0, credit: req.ZeroVatNetCost,
                            $"Return Zero-VAT - {req.ReferenceNumber}",
                            $"إرجاع صفري الضريبة - {req.ReferenceNumber}", seq++));
                }

                // CR Inventory (exempt items)
                if (req.ExemptNetCost > 0 && settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: 0, credit: req.ExemptNetCost,
                        $"Return Exempt - {req.ReferenceNumber}",
                        $"إرجاع معفى - {req.ReferenceNumber}", seq++));
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
                    var absNet    = Math.Abs(netAmount);
                    bool isSurplus = netAmount >= 0;

                    details.Add(Detail(entry.Oid,
                        isSurplus ? settings.InventoryAccountId.Value : adjAccountId.Value,
                        debit: absNet, credit: 0,
                        $"Inventory Adjustment - {req.ReferenceNumber}",
                        $"تسوية مخزون - {req.ReferenceNumber}", seq++));

                    details.Add(Detail(entry.Oid,
                        isSurplus ? adjAccountId.Value : settings.InventoryAccountId.Value,
                        debit: 0, credit: absNet,
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
                        debit: netAmount, credit: 0,
                        $"Expired Write-off - {req.ReferenceNumber}",
                        $"شطب منتهي الصلاحية - {req.ReferenceNumber}", seq++));

                if (settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: 0, credit: netAmount,
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
                        debit: netAmount, credit: 0,
                        $"Damaged Write-off - {req.ReferenceNumber}",
                        $"شطب بضاعة تالفة - {req.ReferenceNumber}", seq++));

                if (settings.InventoryAccountId.HasValue)
                    details.Add(Detail(entry.Oid, settings.InventoryAccountId.Value,
                        debit: 0, credit: netAmount,
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
