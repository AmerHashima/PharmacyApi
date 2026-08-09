using ECommerceIntegration.Domain.Common;
using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Domain.Entities;

public class ECommerceSyncQueue : ECommerceBaseEntity
{
    public Guid StoreId { get; set; }
    public ECommerceProviderType ProviderType { get; set; }
    public string EntityType { get; set; } = null!;
    public string ActionType { get; set; } = null!;
    public Guid? LocalEntityId { get; set; }
    public string? ExternalEntityId { get; set; }
    public string? Payload { get; set; }
    public ECommerceSyncStatus Status { get; set; } = ECommerceSyncStatus.Pending;
    public int RetryCount { get; set; }
    public int MaxRetryCount { get; set; } = 5;
    public string? ErrorMessage { get; set; }
    public DateTime? LastTriedAt { get; set; }
    public DateTime? SyncedAt { get; set; }
}
