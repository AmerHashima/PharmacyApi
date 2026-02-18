using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents a batch/lot of a product with expiry tracking.
/// Essential for pharmaceutical inventory management and traceability.
/// </summary>
[Table("ProductBatches")]
public class ProductBatch : BaseEntity
{
    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Batch/Lot number from manufacturer
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string BatchNumber { get; set; } = string.Empty;

    /// <summary>
    /// Manufacturing date
    /// </summary>
    public DateTime? ManufactureDate { get; set; }

    /// <summary>
    /// Expiry date - critical for pharmaceutical products
    /// </summary>
    [Required]
    public DateTime ExpiryDate { get; set; }

    /// <summary>
    /// Quantity received in this batch
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? ReceivedQuantity { get; set; }

    /// <summary>
    /// Current available quantity in this batch
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? CurrentQuantity { get; set; }

    /// <summary>
    /// Purchase price per unit for this batch
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? PurchasePrice { get; set; }

    /// <summary>
    /// Selling price per unit for this batch
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? SellingPrice { get; set; }

    /// <summary>
    /// FK to AppLookupDetail - Batch status (Active, Expired, Quarantine, etc.)
    /// </summary>
    public Guid? BatchStatusId { get; set; }

    [ForeignKey(nameof(BatchStatusId))]
    public virtual AppLookupDetail? BatchStatus { get; set; }

    /// <summary>
    /// Branch where this batch is stored
    /// </summary>
    [Required]
    public Guid BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch Branch { get; set; } = null!;

    /// <summary>
    /// Supplier who provided this batch
    /// </summary>
    public Guid? SupplierId { get; set; }

    [ForeignKey(nameof(SupplierId))]
    public virtual Stakeholder? Supplier { get; set; }
}
