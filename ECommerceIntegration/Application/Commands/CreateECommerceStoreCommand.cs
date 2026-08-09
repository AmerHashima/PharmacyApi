using MediatR;
using ECommerceIntegration.Domain.Entities;
using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Application.Commands;

public record CreateECommerceStoreCommand(
    ECommerceProviderType ProviderType,
    string? ExternalStoreId,
    string? StoreName,
    string? AccessToken,
    string? RefreshToken,
    DateTime? TokenExpiresAt,
    Guid? BranchId,
    Guid? WarehouseId,
    Guid? PriceListId,
    bool AutoPullOrders,
    bool AutoSyncStock,
    bool AutoSyncPrice,
    bool IsActive) : IRequest<ECommerceStore>;
