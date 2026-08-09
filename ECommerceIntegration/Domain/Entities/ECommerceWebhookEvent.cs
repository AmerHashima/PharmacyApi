using ECommerceIntegration.Domain.Common;
using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Domain.Entities;

public class ECommerceWebhookEvent : ECommerceBaseEntity
{
    public ECommerceProviderType ProviderType { get; set; }
    public Guid? StoreId { get; set; }
    public string EventName { get; set; } = null!;
    public string? ExternalId { get; set; }
    public string Payload { get; set; } = null!;
    public ECommerceSyncStatus ProcessingStatus { get; set; } = ECommerceSyncStatus.Pending;
    public string? ErrorMessage { get; set; }
    public DateTime? ProcessedAt { get; set; }
}
