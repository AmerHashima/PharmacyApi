using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents current inventory levels of products at each branch.
/// Tracks available and reserved quantities.
/// </summary>
[Table("Stock")]
public class Stock : BaseEntity
{
    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    [Required]
    public Guid BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch Branch { get; set; } = null!;

    /// <summary>
    /// Total available quantity in stock
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Quantity reserved for pending orders/transactions
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? ReservedQuantity { get; set; }

    /// <summary>
    /// Calculated: Quantity - ReservedQuantity
    /// </summary>
    [NotMapped]
    public decimal AvailableQuantity => (Quantity ?? 0) - (ReservedQuantity ?? 0);

    /// <summary>
    /// Last stock count date
    /// </summary>
    public DateTime? LastStockCountDate { get; set; }

    /// <summary>
    /// Average cost per unit (weighted average)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? AverageCost { get; set; }
}
