using ECommerceIntegration.Application.Dtos;

namespace ECommerceIntegration.Application.Abstractions;

public interface IECommerceProductSyncService
{
    Task<ProductPullResultDto> PullProductsAsync(
        Guid storeId,
        CancellationToken cancellationToken);

    Task<ProductPushResultDto> PushInitialProductsAsync(
        Guid storeId,
        Guid? branchId,
        Guid? warehouseId,
        Guid? priceListId,
        int batchSize,
        CancellationToken cancellationToken);
}
