using MediatR;

namespace ECommerceIntegration.Application.Commands;

public record PullECommerceOrdersCommand(
    Guid StoreId,
    DateTime FromDate,
    DateTime ToDate) : IRequest<Unit>;
