using Pharmacy.Application.DTOs.AppLookup;
using Pharmacy.Application.DTOs.Common;
using MediatR;

namespace Pharmacy.Application.Queries.AppLookup;

public record GetLookupDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<AppLookupMasterDto>>;