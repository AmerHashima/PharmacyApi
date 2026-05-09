using MediatR;

namespace Pharmacy.Application.Commands.Accounting;

public record DeleteBankAccountCommand(Guid Id) : IRequest<bool>;
