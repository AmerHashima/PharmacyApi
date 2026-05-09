using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Queries.Accounting;

public record GetAccountByIdQuery(Guid Id) : IRequest<AccountDto?>;
