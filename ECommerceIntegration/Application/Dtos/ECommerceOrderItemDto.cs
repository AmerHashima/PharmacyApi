namespace ECommerceIntegration.Application.Dtos;

public class ECommerceOrderItemDto
{
    public string? ExternalProductId { get; set; }
    public string? ExternalVariantId { get; set; }
    public string? ExternalSku { get; set; }
    public string? ExternalBarcode { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal TotalAmount { get; set; }
}
