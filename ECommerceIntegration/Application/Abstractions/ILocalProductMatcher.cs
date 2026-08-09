using ECommerceIntegration.Application.Dtos;

namespace ECommerceIntegration.Application.Abstractions;

public interface ILocalProductMatcher
{
    Task<LocalProductMatchDto?> MatchAsync(
        ECommerceProductDto product,
        CancellationToken cancellationToken);

    Task<LocalProductMatchDto?> GetByIdAsync(
        Guid localProductId,
        CancellationToken cancellationToken);
}
