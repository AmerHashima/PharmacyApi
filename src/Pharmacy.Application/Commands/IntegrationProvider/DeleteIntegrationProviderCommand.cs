using MediatR;

namespace Pharmacy.Application.Commands.IntegrationProvider;

public record DeleteIntegrationProviderCommand(Guid Id) : IRequest<bool>;