using ECommerceIntegration.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Application.Services;

public class ECommerceWebhookProcessor
{
    private readonly IECommerceIntegrationDbContext _db;

    public ECommerceWebhookProcessor(IECommerceIntegrationDbContext db)
    {
        _db = db;
    }

    public async Task ProcessPendingAsync(CancellationToken cancellationToken)
    {
        var pendingEvents = await _db.ECommerceWebhookEvents
            .Where(x => x.ProcessingStatus == ECommerceSyncStatus.Pending)
            .OrderBy(x => x.CreatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);

        foreach (var webhookEvent in pendingEvents)
        {
            webhookEvent.ProcessingStatus = ECommerceSyncStatus.Ignored;
            webhookEvent.ProcessedAt = DateTime.UtcNow;
            webhookEvent.ErrorMessage = "Webhook processor routing is not implemented yet.";
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
