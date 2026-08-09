namespace ECommerceIntegration.Application.Dtos;

public class ECommerceOrderDto
{
    public string ExternalOrderId { get; set; } = null!;
    public string? ExternalOrderNumber { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerMobile { get; set; }
    public string? CustomerEmail { get; set; }
    public string? OrderStatus { get; set; }
    public string? PaymentStatus { get; set; }
    public string? PaymentMethod { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime? ExternalCreatedAt { get; set; }
    public List<ECommerceOrderItemDto> Items { get; set; } = new();
}
