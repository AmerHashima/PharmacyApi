using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents product packaging units and their conversion factors.
/// Each product can have multiple units (e.g., Box → Strip → Tablet).
/// </summary>
[Table("ProductUnits")]
public class ProductUnit : BaseEntity
{
    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    /// <summary>
    /// FK to AppLookupDetail — Package type (Box, Strip, Tablet, etc.)
    /// </summary>
    [Required]
    public Guid? PackageTypeId { get; set; }

    [ForeignKey(nameof(PackageTypeId))]
    public virtual AppLookupDetail? PackageType { get; set; }

    /// <summary>
    /// Number of units inside the larger unit.
    /// Example: Box = 10 Strips, Strip = 10 Tablets
    /// </summary>
    public int ConversionFactor { get; set; }

    /// <summary>
    /// Unit price for this packaging unit
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Barcode specific to this unit
    /// </summary>
    [MaxLength(100)]
    public string? Barcode { get; set; }
}
