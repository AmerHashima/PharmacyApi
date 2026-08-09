using MediatR;
using ECommerceIntegration.Domain.Entities;
using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Application.Commands;

public record ConnectECommerceStoreCommand(
    ECommerceProviderType ProviderType,
    string Code,
    Guid? BranchId,
    Guid? WarehouseId,
    Guid? PriceListId) : IRequest<ECommerceStore>;
