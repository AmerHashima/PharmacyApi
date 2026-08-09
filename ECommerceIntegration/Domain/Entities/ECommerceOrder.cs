using ECommerceIntegration.Domain.Common;
using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Domain.Entities;

public class ECommerceOrder : ECommerceBaseEntity
{
    public Guid StoreId { get; set; }
    public ECommerceStore Store { get; set; } = null!;
    public ECommerceProviderType ProviderType { get; set; }
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
    public Guid? LocalInvoiceId { get; set; }
    public Guid? LocalOrderId { get; set; }
    public ECommerceSyncStatus SyncStatus { get; set; } = ECommerceSyncStatus.Pending;
    public ECommerceOrderLocalStatus LocalStatus { get; set; } = ECommerceOrderLocalStatus.NotConverted;
    public string? ErrorMessage { get; set; }
    public DateTime? ExternalCreatedAt { get; set; }
    public DateTime? SyncedAt { get; set; }

    public ICollection<ECommerceOrderItem> Items { get; set; } = new List<ECommerceOrderItem>();
}
