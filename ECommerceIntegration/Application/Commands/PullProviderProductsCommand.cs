using ECommerceIntegration.Application.Dtos;
using MediatR;

namespace ECommerceIntegration.Application.Commands;

public sealed record PullProviderProductsCommand(Guid StoreId)
    : IRequest<ProductPullResultDto>;
