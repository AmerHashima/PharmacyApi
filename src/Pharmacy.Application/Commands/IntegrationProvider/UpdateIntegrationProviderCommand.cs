using MediatR;
using Pharmacy.Application.DTOs.IntegrationProvider;

namespace Pharmacy.Application.Commands.IntegrationProvider;

public record UpdateIntegrationProviderCommand(UpdateIntegrationProviderDto Dto) : IRequest<IntegrationProviderDto>;