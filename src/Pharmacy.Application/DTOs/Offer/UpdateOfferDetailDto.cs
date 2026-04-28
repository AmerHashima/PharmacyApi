using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Offer;

public class UpdateOfferDetailDto
{
    [Required]
    public Guid Oid { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    // DISCOUNT
    [Range(0, 100)]
    public decimal? DiscountPercent { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? DiscountAmount { get; set; }

    // PACKAGE_PRICE
    [Range(1, int.MaxValue)]
    public int? PackageQuantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? PackagePrice { get; set; }

    // FREE_ITEMS
    [Range(1, int.MaxValue)]
    public int? BuyQuantity { get; set; }

    [Range(1, int.MaxValue)]
    public int? FreeQuantity { get; set; }

    public Guid? FreeProductId { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
