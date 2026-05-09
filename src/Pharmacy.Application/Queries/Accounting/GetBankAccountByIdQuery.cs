using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Queries.Accounting;

public record GetBankAccountByIdQuery(Guid Id) : IRequest<BankAccountDto?>;
