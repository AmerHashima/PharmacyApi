using MediatR;

namespace ECommerceIntegration.Application.Commands;

public record PullECommerceOrderByExternalIdCommand(
    Guid StoreId,
    string ExternalOrderId) : IRequest<Unit>;
