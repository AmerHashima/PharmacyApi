using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Application.Dtos;

namespace ECommerceIntegration.Application.Services;

public class NullLocalProductCatalogReader : ILocalProductCatalogReader
{
    public Task<IReadOnlyList<LocalProductCatalogItemDto>> GetInitialProductsAsync(
        Guid? branchId,
        Guid? warehouseId,
        Guid? priceListId,
        int take,
        int skip,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<LocalProductCatalogItemDto>>(Array.Empty<LocalProductCatalogItemDto>());
    }

    public Task<LocalProductCatalogItemDto?> GetProductByIdAsync(
        Guid productId,
        Guid? productUnitId,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<LocalProductCatalogItemDto?>(null);
    }
}
