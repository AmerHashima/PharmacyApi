using MediatR;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Application.Queries;

public record GetECommerceOrderListQuery(Guid? StoreId) : IRequest<IEnumerable<ECommerceOrder>>;
