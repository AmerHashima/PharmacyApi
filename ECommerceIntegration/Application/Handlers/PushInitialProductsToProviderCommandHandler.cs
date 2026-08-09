using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Application.Commands;
using ECommerceIntegration.Application.Dtos;
using MediatR;

namespace ECommerceIntegration.Application.Handlers;

public class PushInitialProductsToProviderCommandHandler
    : IRequestHandler<PushInitialProductsToProviderCommand, ProductPushResultDto>
{
    private readonly IECommerceProductSyncService _productSyncService;

    public PushInitialProductsToProviderCommandHandler(IECommerceProductSyncService productSyncService)
    {
        _productSyncService = productSyncService;
    }

    public Task<ProductPushResultDto> Handle(
        PushInitialProductsToProviderCommand request,
        CancellationToken cancellationToken)
    {
        return _productSyncService.PushInitialProductsAsync(
            request.StoreId,
            request.BranchId,
            request.WarehouseId,
            request.PriceListId,
            request.BatchSize,
            cancellationToken);
    }
}
