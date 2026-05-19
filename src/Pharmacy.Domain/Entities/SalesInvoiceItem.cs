using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents a single line item in a sales invoice.
/// </summary>
[Table("SalesInvoiceItems")]
public class SalesInvoiceItem : BaseEntity
{
    [Required]
    public Guid InvoiceId { get; set; }

    [ForeignKey(nameof(InvoiceId))]
    public virtual SalesInvoice Invoice { get; set; } = null!;

    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    // ── Position ──────────────────────────────────────────────────────────

    /// <summary>Line sequence number (1-based) within the invoice.</summary>
    public int LineNumber { get; set; }

    // ── Quantity ──────────────────────────────────────────────────────────

    /// <summary>Quantity sold on this line.</summary>
    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal Quantity { get; set; }

    /// <summary>Quantity still available for return (decremented on each return).</summary>
    [Column(TypeName = "decimal(18,4)")]
    public decimal RemainingQuantity { get; set; }

    /// <summary>Quantity already returned against this line.</summary>
    [Column(TypeName = "decimal(18,4)")]
    public decimal ReturnedQuantity { get; set; }

    // ── Pricing ───────────────────────────────────────────────────────────

    /// <summary>Selling price per unit at time of sale.</summary>
    [Column(TypeName = "decimal(18,4)")]
    public decimal? UnitPrice { get; set; }

    /// <summary>Cost price per unit at time of sale (used for COGS and profit).</summary>
    [Column(TypeName = "decimal(18,4)")]
    public decimal? CostPrice { get; set; }

    // ── Discount ──────────────────────────────────────────────────────────

    /// <summary>Discount percentage on this line.</summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal? DiscountPercent { get; set; }

    /// <summary>Calculated discount amount on this line.</summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountAmount { get; set; }

    // ── Tax (VAT) ─────────────────────────────────────────────────────────

    /// <summary>VAT / tax percentage applied to this line (e.g. 15.00 for 15%).</summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal? TaxPercent { get; set; }

    /// <summary>Calculated tax amount for this line (NetPrice * TaxPercent / 100).</summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? TaxAmount { get; set; }

    // ── Totals ────────────────────────────────────────────────────────────

    /// <summary>Net price before tax: (Quantity * UnitPrice) - DiscountAmount.</summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? NetPrice { get; set; }

    /// <summary>Gross total including tax: NetPrice + TaxAmount.</summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalPrice { get; set; }

    // ── Batch / Serial ────────────────────────────────────────────────────

    [MaxLength(50)]
    public string? BatchNumber { get; set; }

    [MaxLength(100)]
    public string? SerialNumber { get; set; }

    public DateTime? ExpiryDate { get; set; }

    // ── Return flag ───────────────────────────────────────────────────────

    /// <summary>True when this line was generated as a free-item line from an offer.</summary>
    public bool IsFreeItem { get; set; }

    // ── Offer ─────────────────────────────────────────────────────────────

    /// <summary>FK to OfferDetail — the specific offer line applied when this item was sold.</summary>
    public Guid? OfferDetailId { get; set; }

    [ForeignKey(nameof(OfferDetailId))]
    public virtual OfferDetail? OfferDetail { get; set; }

    /// <summary>Human-readable offer name snapshot (preserved if offer is later deleted).</summary>
    [MaxLength(300)]
    public string? OfferNameSnapshot { get; set; }

    // ── Misc ──────────────────────────────────────────────────────────────

    [MaxLength(500)]
    public string? Notes { get; set; }
}
