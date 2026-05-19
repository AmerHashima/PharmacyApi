using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.SalesInvoice;

/// <summary>
/// DTO for creating a new Sales Invoice (POS transaction)
/// </summary>
public class CreateSalesInvoiceDto
{
    [Required(ErrorMessage = "Branch ID is required")]
    public Guid BranchId { get; set; }

    /// <summary>
    /// Provide an existing CustomerId OR the convenience fields below.
    /// If both are omitted the invoice is linked to the default Cash Patient.
    /// </summary>
    public Guid? CustomerId { get; set; }

    // ── Convenience fields — used to auto-create / resolve a Customer ─────
    [MaxLength(200)]
    public string? CustomerName { get; set; }

    [MaxLength(50)]
    public string? CustomerPhone { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(100)]
    public string? CustomerEmail { get; set; }

    /// <summary>FK to Doctor — prescribing or referring doctor for this invoice.</summary>
    public Guid? DoctorId { get; set; }

    [Range(0, 100, ErrorMessage = "Discount percent must be between 0 and 100")]
    public decimal? DiscountPercent { get; set; }

    /// <summary>
    /// Header-level tax percentage applied to all lines that don't have their own TaxPercent.
    /// </summary>
    [Range(0, 100, ErrorMessage = "Tax percent must be between 0 and 100")]
    public decimal? TaxPercent { get; set; }

    /// <summary>
    /// Explicit tax amount override. When provided, used directly instead of computing from TaxPercent.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Tax amount must be >= 0")]
    public decimal? TaxAmount { get; set; }

    /// <summary>Amount tendered by customer.</summary>
    [Range(0, double.MaxValue, ErrorMessage = "Paid amount must be >= 0")]
    public decimal? PaidAmount { get; set; }

    /// <summary>Change returned to customer.</summary>
    [Range(0, double.MaxValue, ErrorMessage = "Change amount must be >= 0")]
    public decimal? ChangeAmount { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public Guid? PaymentMethodId { get; set; }

    public Guid? CashierId { get; set; }

    [MaxLength(50)]
    public string? PrescriptionNumber { get; set; }

    [MaxLength(200)]
    public string? DoctorName { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Optional: Account to debit (AR / Cash). Resolved from branch setup if omitted.
    /// </summary>
    public Guid? ReceivableAccountId { get; set; }

    /// <summary>
    /// Optional: Account to credit (Sales Revenue). Resolved from branch setup if omitted.
    /// </summary>
    public Guid? RevenueAccountId { get; set; }

    [Required(ErrorMessage = "At least one item is required")]
    [MinLength(1, ErrorMessage = "At least one item is required")]
    public List<CreateSalesInvoiceItemDto> Items { get; set; } = new();
}

/// <summary>
/// DTO for creating a sales invoice line item
/// </summary>
public class CreateSalesInvoiceItemDto
{
    [Required(ErrorMessage = "Product ID is required")]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public decimal Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    [Range(0, 100, ErrorMessage = "Discount percent must be between 0 and 100")]
    public decimal? DiscountPercent { get; set; }

    /// <summary>VAT / tax percentage for this line (e.g. 15 for 15%). Overrides the header TaxPercent.</summary>
    [Range(0, 100, ErrorMessage = "Tax percent must be between 0 and 100")]
    public decimal? TaxPercent { get; set; }

    /// <summary>Explicit line-level tax amount override. Overrides computation from TaxPercent when provided.</summary>
    [Range(0, double.MaxValue)]
    public decimal? TaxAmount { get; set; }

    /// <summary>Cost price per unit — provided by frontend when known (e.g. from stock batch lookup).</summary>
    [Range(0, double.MaxValue)]
    public decimal? CostPrice { get; set; }

    /// <summary>Line sequence number. Auto-assigned if not provided.</summary>
    public int? LineNumber { get; set; }

    [MaxLength(50)]
    public string? BatchNumber { get; set; }
    public string? SerialNumber { get; set; }

    public DateTime? ExpiryDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>Optional: pass the OfferDetail ID to apply an offer on this line.</summary>
    public Guid? OfferDetailId { get; set; }
}
