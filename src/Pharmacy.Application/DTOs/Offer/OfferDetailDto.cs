namespace Pharmacy.Application.DTOs.Offer;

public class OfferDetailDto
{
    public Guid Oid { get; set; }
    public Guid OfferMasterId { get; set; }

    public Guid ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductNameAr { get; set; }
    public string? ProductBarcode { get; set; }

    // DISCOUNT
    public decimal? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }

    // PACKAGE_PRICE
    public int? PackageQuantity { get; set; }
    public decimal? PackagePrice { get; set; }

    // FREE_ITEMS
    public int? BuyQuantity { get; set; }
    public int? FreeQuantity { get; set; }
    public Guid? FreeProductId { get; set; }
    public string? FreeProductName { get; set; }
    public string? FreeProductNameAr { get; set; }

    public string? Notes { get; set; }
}
