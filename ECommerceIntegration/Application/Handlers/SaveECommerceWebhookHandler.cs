using ECommerceIntegration.Application.Abstractions;
using MediatR;
using ECommerceIntegration.Application.Commands;
using ECommerceIntegration.Domain.Entities;
using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Application.Handlers;

public class SaveECommerceWebhookHandler : IRequestHandler<SaveECommerceWebhookCommand, Unit>
{
    private readonly IECommerceIntegrationDbContext _db;

    public SaveECommerceWebhookHandler(IECommerceIntegrationDbContext db)
    {
        _db = db;
    }

    public async Task<Unit> Handle(SaveECommerceWebhookCommand request, CancellationToken cancellationToken)
    {
        _db.ECommerceWebhookEvents.Add(new ECommerceWebhookEvent
        {
            Oid = Guid.NewGuid(),
            ProviderType = request.ProviderType,
            EventName = request.EventName,
            ExternalId = request.ExternalId,
            Payload = request.Payload,
            ProcessingStatus = ECommerceSyncStatus.Pending
        });

        await _db.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
