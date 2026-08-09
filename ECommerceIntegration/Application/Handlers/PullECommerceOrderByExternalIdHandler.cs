using MediatR;
using ECommerceIntegration.Application.Commands;
using ECommerceIntegration.Application.Abstractions;

namespace ECommerceIntegration.Application.Handlers;

public class PullECommerceOrderByExternalIdHandler : IRequestHandler<PullECommerceOrderByExternalIdCommand, Unit>
{
    private readonly IECommerceOrderSyncService _orderSyncService;

    public PullECommerceOrderByExternalIdHandler(IECommerceOrderSyncService orderSyncService)
    {
        _orderSyncService = orderSyncService;
    }

    public async Task<Unit> Handle(PullECommerceOrderByExternalIdCommand request, CancellationToken cancellationToken)
    {
        await _orderSyncService.PullOrderByExternalIdAsync(
            request.StoreId,
            request.ExternalOrderId,
            cancellationToken);

        return Unit.Value;
    }
}
