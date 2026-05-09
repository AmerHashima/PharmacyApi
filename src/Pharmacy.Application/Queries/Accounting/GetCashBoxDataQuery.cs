using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;

namespace Pharmacy.Application.Queries.Accounting;

public record GetCashBoxDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<CashBoxDto>>;
