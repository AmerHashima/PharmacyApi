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
}
