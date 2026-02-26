using MediatR;
using Pharmacy.Application.DTOs.IntegrationProvider;

namespace Pharmacy.Application.Commands.IntegrationProvider;

public record CreateIntegrationProviderCommand(CreateIntegrationProviderDto Dto) : IRequest<IntegrationProviderDto>;