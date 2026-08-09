using ECommerceIntegration.Application.Dtos;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Application.Abstractions;

public interface IECommerceProductMappingService
{
    Task<IReadOnlyList<ECommerceProductMapping>> ImportProviderProductsAsync(
        Guid storeId,
        CancellationToken cancellationToken);

    Task<ECommerceProductMapping?> MatchProductAsync(
        Guid storeId,
        ECommerceProductDto product,
        CancellationToken cancellationToken);

    Task<ECommerceProductMapping> MapProductAsync(
        Guid mappingId,
        Guid localProductId,
        CancellationToken cancellationToken);
}
