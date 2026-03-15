using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents line items in a return invoice.
/// Contains product details, quantity, and pricing for each item returned.
/// </summary>
[Table("ReturnInvoiceItems")]
public class ReturnInvoiceItem : BaseEntity
{
    [Required]
    public Guid ReturnInvoiceId { get; set; }

    [ForeignKey(nameof(ReturnInvoiceId))]
    public virtual ReturnInvoice ReturnInvoice { get; set; } = null!;

    /// <summary>
    /// FK to the original sales invoice item being returned (optional for partial returns)
    /// </summary>
    public Guid? OriginalInvoiceItemId { get; set; }

    [ForeignKey(nameof(OriginalInvoiceItemId))]
    public virtual SalesInvoiceItem? OriginalInvoiceItem { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Quantity returned
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Unit price at time of original sale
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// Discount percentage on this item
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal? DiscountPercent { get; set; }

    /// <summary>
    /// Discount amount on this item
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// Total refund price for this line item (Quantity * UnitPrice - Discount)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalPrice { get; set; }

    /// <summary>
    /// Cost price at time of sale (for profit calculation)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? CostPrice { get; set; }

    /// <summary>
    /// Batch number from which the item was dispensed
    /// </summary>
    [MaxLength(50)]
    public string? BatchNumber { get; set; }

    /// <summary>
    /// Expiry date of the returned batch
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Notes specific to this item
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }
}
