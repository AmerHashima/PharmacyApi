using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents a promotional offer header.
/// OfferTypeId determines which columns in OfferDetail are active:
///   DISCOUNT      → DiscountPercent / DiscountAmount
///   PACKAGE_PRICE → PackageQuantity / PackagePrice
///   FREE_ITEMS    → BuyQuantity / FreeQuantity / FreeProductId
/// </summary>
[Table("OfferMasters")]
public class OfferMaster : BaseEntity
{
    [Required]
    [MaxLength(300)]
    public string OfferNameAr { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string OfferNameEn { get; set; } = string.Empty;

    /// <summary>FK to AppLookupDetail — OFFER_TYPE (DISCOUNT / PACKAGE_PRICE / FREE_ITEMS).</summary>
    [Required]
    public Guid OfferTypeId { get; set; }

    [ForeignKey(nameof(OfferTypeId))]
    public virtual AppLookupDetail OfferType { get; set; } = null!;

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    /// <summary>Optional: restrict the offer to a specific branch. Null = all branches.</summary>
    public Guid? BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch? Branch { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public virtual ICollection<OfferDetail> OfferDetails { get; set; } = new List<OfferDetail>();
}
