using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Queries.Rsd;

public record GetRsdOperationLogDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<RsdOperationLogDto>>;
