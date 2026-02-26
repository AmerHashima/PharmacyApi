using MediatR;
using Pharmacy.Application.DTOs.IntegrationProvider;

namespace Pharmacy.Application.Queries.IntegrationProvider;

public record GetIntegrationProviderByIdQuery(Guid Id) : IRequest<IntegrationProviderDto?>;