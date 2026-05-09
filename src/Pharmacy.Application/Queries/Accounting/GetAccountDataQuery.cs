using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;

namespace Pharmacy.Application.Queries.Accounting;

public record GetAccountDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<AccountDto>>;
