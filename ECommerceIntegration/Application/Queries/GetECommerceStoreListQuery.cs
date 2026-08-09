using MediatR;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Application.Queries;

public record GetECommerceStoreListQuery : IRequest<IEnumerable<ECommerceStore>>;
