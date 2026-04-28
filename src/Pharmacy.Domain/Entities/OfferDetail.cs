using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents a single product line within a promotional offer.
/// Columns used depend on the parent OfferMaster.OfferTypeId:
///   DISCOUNT      → DiscountPercent / DiscountAmount
///   PACKAGE_PRICE → PackageQuantity / PackagePrice
///   FREE_ITEMS    → BuyQuantity / FreeQuantity / FreeProductId
/// </summary>
[Table("OfferDetails")]
public class OfferDetail : BaseEntity
{
    [Required]
    public Guid OfferMasterId { get; set; }

    [ForeignKey(nameof(OfferMasterId))]
    public virtual OfferMaster OfferMaster { get; set; } = null!;

    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    // ── DISCOUNT ──────────────────────────────────────────
    [Column(TypeName = "decimal(5,2)")]
    public decimal? DiscountPercent { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountAmount { get; set; }

    // ── PACKAGE_PRICE ─────────────────────────────────────
    public int? PackageQuantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? PackagePrice { get; set; }

    // ── FREE_ITEMS ────────────────────────────────────────
    public int? BuyQuantity { get; set; }

    public int? FreeQuantity { get; set; }

    /// <summary>Free product may differ from the purchased product. Null = same as ProductId.</summary>
    public Guid? FreeProductId { get; set; }

    [ForeignKey(nameof(FreeProductId))]
    public virtual Product? FreeProduct { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
