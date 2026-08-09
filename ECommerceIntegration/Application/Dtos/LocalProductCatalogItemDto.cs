namespace ECommerceIntegration.Application.Dtos;

public class LocalProductCatalogItemDto
{
    public Guid ProductId { get; set; }
    public Guid? ProductUnitId { get; set; }
    public string? ProductCode { get; set; }
    public string ProductNameAr { get; set; } = string.Empty;
    public string? ProductNameEn { get; set; }
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
    public string? Barcode { get; set; }
    public string? GTIN { get; set; }
    public string? SKU { get; set; }
    public decimal Price { get; set; }
    public decimal? CostPrice { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? VatPercent { get; set; }
    public bool IsVatIncluded { get; set; }
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public Dictionary<string, string>? ExtraData { get; set; }
}
