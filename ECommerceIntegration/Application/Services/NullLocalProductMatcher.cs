using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Application.Dtos;

namespace ECommerceIntegration.Application.Services;

public class NullLocalProductMatcher : ILocalProductMatcher
{
    public Task<LocalProductMatchDto?> MatchAsync(
        ECommerceProductDto product,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<LocalProductMatchDto?>(null);
    }

    public Task<LocalProductMatchDto?> GetByIdAsync(
        Guid localProductId,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<LocalProductMatchDto?>(null);
    }
}
