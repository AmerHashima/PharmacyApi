using MediatR;

namespace Pharmacy.Application.Commands.Accounting;

public record DeleteCashBoxCommand(Guid Id) : IRequest<bool>;
