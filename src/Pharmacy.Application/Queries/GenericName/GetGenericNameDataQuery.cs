using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.GenericName;

namespace Pharmacy.Application.Queries.GenericName;

public record GetGenericNameDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<GenericNameDto>>;
