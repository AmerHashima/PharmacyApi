using MediatR;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Application.Commands;

public record UpdateECommerceStoreSettingsCommand(
    Guid Id,
    Guid? BranchId,
    Guid? WarehouseId,
    Guid? PriceListId,
    bool AutoPullOrders,
    bool AutoSyncStock,
    bool AutoSyncPrice,
    bool IsActive) : IRequest<ECommerceStore?>;
