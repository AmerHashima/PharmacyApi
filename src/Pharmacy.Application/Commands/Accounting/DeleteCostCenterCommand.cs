using MediatR;

namespace Pharmacy.Application.Commands.Accounting;

public record DeleteCostCenterCommand(Guid Id) : IRequest<bool>;
