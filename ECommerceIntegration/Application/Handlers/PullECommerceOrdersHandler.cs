using MediatR;
using ECommerceIntegration.Application.Commands;
using ECommerceIntegration.Application.Abstractions;

namespace ECommerceIntegration.Application.Handlers;

public class PullECommerceOrdersHandler : IRequestHandler<PullECommerceOrdersCommand, Unit>
{
    private readonly IECommerceOrderSyncService _orderSyncService;

    public PullECommerceOrdersHandler(IECommerceOrderSyncService orderSyncService)
    {
        _orderSyncService = orderSyncService;
    }

    public async Task<Unit> Handle(PullECommerceOrdersCommand request, CancellationToken cancellationToken)
    {
        await _orderSyncService.PullOrdersAsync(
            request.StoreId,
            request.FromDate,
            request.ToDate,
            cancellationToken);

        return Unit.Value;
    }
}
