using ECommerceIntegration.Application.Dtos;

namespace ECommerceIntegration.Application.Abstractions;

public interface ILocalProductCatalogReader
{
    Task<IReadOnlyList<LocalProductCatalogItemDto>> GetInitialProductsAsync(
        Guid? branchId,
        Guid? warehouseId,
        Guid? priceListId,
        int take,
        int skip,
        CancellationToken cancellationToken);

    Task<LocalProductCatalogItemDto?> GetProductByIdAsync(
        Guid productId,
        Guid? productUnitId,
        CancellationToken cancellationToken);
}
