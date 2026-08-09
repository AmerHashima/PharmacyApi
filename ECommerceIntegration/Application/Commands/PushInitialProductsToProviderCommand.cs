using ECommerceIntegration.Application.Dtos;
using MediatR;

namespace ECommerceIntegration.Application.Commands;

public sealed record PushInitialProductsToProviderCommand(
    Guid StoreId,
    Guid? BranchId,
    Guid? WarehouseId,
    Guid? PriceListId,
    int BatchSize) : IRequest<ProductPushResultDto>;
