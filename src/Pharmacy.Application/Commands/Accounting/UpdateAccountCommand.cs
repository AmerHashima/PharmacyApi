using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record UpdateAccountCommand(UpdateAccountDto Account) : IRequest<AccountDto>;
