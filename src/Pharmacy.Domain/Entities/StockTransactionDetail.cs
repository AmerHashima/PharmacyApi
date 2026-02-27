using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Detail lines for stock transactions - tracks individual products in each transaction
/// Each transaction header (StockTransaction) can have multiple detail lines
/// </summary>
[Table("StockTransactionDetails")]
public class StockTransactionDetail : BaseEntity
{
    /// <summary>
    /// FK to parent StockTransaction (header)
    /// </summary>
    [Required]
    public Guid StockTransactionId { get; set; }

    [ForeignKey(nameof(StockTransactionId))]
    public virtual StockTransaction StockTransaction { get; set; } = null!;

    /// <summary>
    /// Product being transacted
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// Quantity being transacted for this product
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// GTIN barcode for product verification
    /// </summary>
    [MaxLength(50)]
    public string? Gtin { get; set; }

    /// <summary>
    /// Batch/Lot number for traceability
    /// </summary>
    [MaxLength(50)]
    public string? BatchNumber { get; set; }

    /// <summary>
    /// Expiry date of this specific batch
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Serial number for serialized items (pharmaceuticals)
    /// </summary>
    [MaxLength(100)]
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Unit cost at time of transaction
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? UnitCost { get; set; }

    /// <summary>
    /// Total cost for this line (Quantity × UnitCost)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalCost { get; set; }

    /// <summary>
    /// Line number for ordering within the transaction
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// Notes specific to this line item
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }
}
