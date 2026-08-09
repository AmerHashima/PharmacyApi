using ECommerceIntegration.Application.Dtos;
using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Application.Abstractions;

public interface IECommerceProvider
{
    ECommerceProviderType ProviderType { get; }

    Task<ECommerceTokenDto> ExchangeCodeAsync(string code, CancellationToken cancellationToken);

    Task<ECommerceTokenDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);

    Task<IReadOnlyList<ECommerceProductDto>> GetProductsAsync(
        string accessToken,
        CancellationToken cancellationToken);

    Task<ExternalProductPushResultDto> CreateProductAsync(
        string accessToken,
        LocalProductCatalogItemDto product,
        CancellationToken cancellationToken);

    Task<ExternalProductPushResultDto> UpdateProductAsync(
        string accessToken,
        string externalProductId,
        string? externalVariantId,
        LocalProductCatalogItemDto product,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ECommerceOrderDto>> GetOrdersAsync(
        string accessToken,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken);

    Task<ECommerceOrderDto?> GetOrderByIdAsync(
        string accessToken,
        string externalOrderId,
        CancellationToken cancellationToken);

    Task UpdateStockAsync(
        string accessToken,
        string externalProductId,
        string? externalVariantId,
        decimal quantity,
        CancellationToken cancellationToken);

    Task UpdatePriceAsync(
        string accessToken,
        string externalProductId,
        string? externalVariantId,
        decimal price,
        CancellationToken cancellationToken);
}
