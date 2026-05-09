using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record CreateBankAccountCommand(CreateBankAccountDto BankAccount) : IRequest<BankAccountDto>;
