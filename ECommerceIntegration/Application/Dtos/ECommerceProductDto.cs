namespace ECommerceIntegration.Application.Dtos;

public class ECommerceProductDto
{
    public string ExternalProductId { get; set; } = null!;
    public string? ExternalVariantId { get; set; }
    public string? Sku { get; set; }
    public string? Barcode { get; set; }
    public string? Gtin { get; set; }
    public string? ProductName { get; set; }
    public decimal? Price { get; set; }
    public decimal? StockQuantity { get; set; }
}
