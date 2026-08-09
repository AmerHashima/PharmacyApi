using ECommerceIntegration.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceIntegration.Application.Abstractions;

public interface IECommerceIntegrationDbContext
{
    DbSet<ECommerceStore> ECommerceStores { get; }
    DbSet<ECommerceProductMapping> ECommerceProductMappings { get; }
    DbSet<ECommerceOrder> ECommerceOrders { get; }
    DbSet<ECommerceOrderItem> ECommerceOrderItems { get; }
    DbSet<ECommerceWebhookEvent> ECommerceWebhookEvents { get; }
    DbSet<ECommerceSyncQueue> ECommerceSyncQueues { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
