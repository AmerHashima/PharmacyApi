using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record CreateAccountCommand(CreateAccountDto Account) : IRequest<AccountDto>;
