using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Application.Interfaces;

// ─────────────────────────────────────────────────────────────────────────────
// VAT Category Classification
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Classifies items by VAT treatment for correct accounting posting.
/// </summary>
public enum VatCategory
{
    /// <summary>Standard VAT rate applies (e.g., 15%) — posts to VatOutputAccountId</summary>
    Taxable = 1,

    /// <summary>Zero-rated (0% VAT, but VAT-registered) — posts to ZeroRatedSalesAccountId, NO VAT account entry</summary>
    ZeroRated = 2,

    /// <summary>Exempt from VAT entirely — posts to ExemptSalesAccountId, NO VAT account entry</summary>
    Exempt = 3
}

// ─────────────────────────────────────────────────────────────────────────────
// Enhanced Line Item with VAT Classification
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Complete line-level detail for sales invoice journal posting.
/// Supports mixed VAT invoices with item-level classification.
/// </summary>
public record SalesInvoiceLineItem(
    Guid ProductId,
    string ProductName,
    VatCategory VatCategory,
    decimal Quantity,
    decimal UnitPrice,
    decimal LineDiscountAmount,
    decimal NetPrice,           // (Qty * UnitPrice) - LineDiscountAmount
    decimal TaxPercent,
    decimal TaxAmount,
    decimal TotalPrice,         // NetPrice + TaxAmount
    decimal CostPrice,
    int LineNumber,
    bool IsFreeItem);

// ─────────────────────────────────────────────────────────────────────────────
// Payment Method Detail for Partial/Mixed Payments
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Represents a single payment method applied to an invoice.
/// Supports mixed payments (e.g., 500 CASH + 300 BANK + 200 CREDIT).
/// </summary>
public record PaymentMethodDetail(
    string MethodCode,  // "CASH" | "BANK" | "CREDIT"
    decimal Amount,
    Guid? BankAccountId = null);  // Optional: specific bank account for BANK payments

// ─────────────────────────────────────────────────────────────────────────────
// Request / Result records
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// All data the journal posting service needs to post a sales invoice.
/// NOW SUPPORTS: Mixed VAT, item-level classification, partial/mixed payments.
/// </summary>
public record SalesInvoicePostingRequest(
    Guid InvoiceOid,
    Guid BranchId,
    Guid? FiscalYearId,
    string InvoiceNumber,
    DateTime InvoiceDate,
    IReadOnlyList<SalesInvoiceLineItem> Items,
    decimal InvoiceDiscountAmount,      // Header-level discount (if any)
    decimal TotalAmount,                // Grand total (after all discounts + tax)
    IReadOnlyList<PaymentMethodDetail> Payments,  // One or more payment methods
    Guid? CustomerId);

/// <summary>Result of a successful sales invoice posting.</summary>
public record SalesInvoicePostingResult(JournalEntry Entry);

// ─────────────────────────────────────────────────────────────────────────────
// Stock Transaction Posting
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Single line item inside a stock transaction.
/// </summary>
public record StockTransactionLineItem(
    Guid   ProductId,
    string ProductName,
    decimal Quantity,
    decimal UnitCost,
    decimal TotalCost,
    int    LineNumber);

/// <summary>
/// All data the journal posting service needs to post a stock transaction.
///
/// Journal rules per type:
///   IN         → DR Inventory            CR Accounts Payable / Purchase
///   OUT        → DR COGS / InventoryLoss  CR Inventory
///   TRANSFER   → DR StockTransfer (To)   CR Inventory (From)
///   RETURN     → DR Accounts Payable     CR Inventory   (purchase return)
///   ADJUSTMENT → DR/CR Inventory         CR/DR InventoryAdjustment
///   EXPIRED    → DR ExpiredItems          CR Inventory
///   DAMAGED    → DR DamagedInventory      CR Inventory
/// </summary>
public record StockTransactionPostingRequest(
    Guid   TransactionOid,
    Guid   BranchId,
    Guid?  FiscalYearId,
    string ReferenceNumber,
    DateTime TransactionDate,
    string TypeCode,                           // "IN" | "OUT" | "TRANSFER" | "RETURN" | "ADJUSTMENT" | "EXPIRED" | "DAMAGED"
    IReadOnlyList<StockTransactionLineItem> Items,
    Guid?   SupplierId       = null,           // Used for IN / RETURN (payable account resolution)

    // ── Purchase invoice cost breakdown (used for IN / RETURN journal grouping) ──
    // DR Inventory = TaxableNetCost + ZeroVatNetCost + ExemptNetCost
    // DR VAT Input = TaxableVatAmount  (ONLY for taxable items; zero-vat and exempt produce no VAT line)
    // GrossTotal   = (TaxableNetCost + ZeroVatNetCost + ExemptNetCost) + TaxableVatAmount
    decimal TaxableNetCost   = 0,              // Σ NetCost of items with TaxPercent > 0
    decimal ZeroVatNetCost   = 0,              // Σ NetCost of items with TaxPercent == 0 (zero-rated)
    decimal ExemptNetCost    = 0,              // Σ NetCost of items fully exempt from VAT
    decimal TaxableVatAmount = 0,              // Σ TaxAmount of taxable items ONLY — posted to VAT Input
    decimal PayedAmount      = 0);             // Amount already paid — CR Cash, remainder → CR Supplier Payable

/// <summary>All data needed to post a return invoice reversal.</summary>
public record ReturnInvoicePostingRequest(
    Guid ReturnInvoiceOid,
    Guid BranchId,
    Guid? FiscalYearId,
    string ReturnNumber,
    DateTime ReturnDate,
    IReadOnlyList<SalesInvoiceLineItem> Items,  // Item-level detail for category-aware reversals
    decimal TotalAmount,
    IReadOnlyList<PaymentMethodDetail> RefundMethods,  // How refund is issued
    Guid? CustomerId);

/// <summary>All data needed to post a stock transaction return journal entry.</summary>
public record StockTransactionReturnPostingRequest(
    Guid   ReturnOid,
    Guid   BranchId,
    Guid?  FiscalYearId,
    string ReferenceNumber,
    DateTime TransactionDate,
    string TypeCode,
    IReadOnlyList<StockTransactionLineItem> Items,
    Guid?   SupplierId       = null,
    decimal TaxableNetCost   = 0,
    decimal ZeroVatNetCost   = 0,
    decimal ExemptNetCost    = 0,
    decimal TaxableVatAmount = 0,
    decimal PayedAmount      = 0);

// ─────────────────────────────────────────────────────────────────────────────
// Service Interface
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Production-grade ERP accounting engine for sales, returns, COGS, VAT.
/// Supports mixed VAT invoices, partial payments, item-level classification.
/// </summary>
public interface IJournalPostingService
{
    /// <summary>
    /// Validates that all required accounting accounts are configured for the branch
    /// before allowing a sales invoice to be created.
    /// Throws <see cref="InvalidOperationException"/> listing every missing account.
    /// </summary>
    Task ValidateSalesAccountingSetupAsync(Guid branchId, CancellationToken ct = default);

    /// <summary>
    /// Validates that all required accounting accounts are configured for the branch
    /// before allowing a return invoice to be created.
    /// </summary>
    Task ValidateReturnAccountingSetupAsync(Guid branchId, CancellationToken ct = default);

    /// <summary>
    /// Validates that all required accounting accounts are configured for the branch
    /// before allowing a stock transaction to be created.
    /// </summary>
    Task ValidateStockTransactionAccountingSetupAsync(Guid branchId, string typeCode, CancellationToken ct = default);

    /// <summary>
    /// Posts a sales invoice with full double-entry accounting.
    /// Supports mixed VAT (taxable/zero-rated/exempt in one invoice).
    /// Supports partial/mixed payments.
    /// Creates item-level COGS entries for audit trail.
    /// </summary>
    Task<SalesInvoicePostingResult> PostSalesInvoiceAsync(
        SalesInvoicePostingRequest req,
        CancellationToken ct = default);

    /// <summary>
    /// Reverses a return invoice with category-aware logic.
    /// Properly reverses sales by VAT category and COGS by item.
    /// </summary>
    Task<JournalEntry> PostReturnInvoiceAsync(
        ReturnInvoicePostingRequest req,
        CancellationToken ct = default);

    /// <summary>
    /// Posts a stock transaction (IN / OUT / TRANSFER / RETURN / ADJUSTMENT / EXPIRED / DAMAGED).
    /// Single balanced journal entry per transaction.
    /// For IN/RETURN with PayedAmount &gt; 0, cash split is included inline:
    ///   CR Cash = PayedAmount, CR Supplier Payable = GrossTotal - PayedAmount.
    /// </summary>
    Task<JournalEntry> PostStockTransactionAsync(
        StockTransactionPostingRequest req,
        CancellationToken ct = default);

    /// <summary>
    /// Posts a stock transaction return to the journal and stamps JournalEntryId on the return record.
    /// </summary>
    Task<JournalEntry> PostStockTransactionReturnAsync(
        StockTransactionReturnPostingRequest req,
        CancellationToken ct = default);
}
