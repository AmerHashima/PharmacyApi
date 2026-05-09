using MediatR;

namespace Pharmacy.Application.Commands.Accounting;

public record DeleteAccountCommand(Guid Id) : IRequest<bool>;
