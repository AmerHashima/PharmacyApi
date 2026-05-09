using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Queries.Accounting;

public record GetCashBoxByIdQuery(Guid Id) : IRequest<CashBoxDto?>;
