using MediatR;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Application.Queries;

public record GetECommerceStoreByIdQuery(Guid Id) : IRequest<ECommerceStore?>;
