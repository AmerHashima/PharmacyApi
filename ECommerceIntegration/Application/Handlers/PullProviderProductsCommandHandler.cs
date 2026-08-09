using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Application.Commands;
using ECommerceIntegration.Application.Dtos;
using MediatR;

namespace ECommerceIntegration.Application.Handlers;

public class PullProviderProductsCommandHandler
    : IRequestHandler<PullProviderProductsCommand, ProductPullResultDto>
{
    private readonly IECommerceProductSyncService _productSyncService;

    public PullProviderProductsCommandHandler(IECommerceProductSyncService productSyncService)
    {
        _productSyncService = productSyncService;
    }

    public Task<ProductPullResultDto> Handle(
        PullProviderProductsCommand request,
        CancellationToken cancellationToken)
    {
        return _productSyncService.PullProductsAsync(request.StoreId, cancellationToken);
    }
}
