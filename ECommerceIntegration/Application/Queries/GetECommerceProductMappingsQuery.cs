using ECommerceIntegration.Domain.Entities;
using MediatR;

namespace ECommerceIntegration.Application.Queries;

public sealed record GetECommerceProductMappingsQuery(Guid? StoreId)
    : IRequest<IReadOnlyList<ECommerceProductMapping>>;
