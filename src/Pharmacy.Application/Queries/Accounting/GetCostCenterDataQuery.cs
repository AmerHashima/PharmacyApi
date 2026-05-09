using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;

namespace Pharmacy.Application.Queries.Accounting;

public record GetCostCenterDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<CostCenterDto>>;
