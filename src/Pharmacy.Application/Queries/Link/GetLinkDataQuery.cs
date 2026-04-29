using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Link;

namespace Pharmacy.Application.Queries.Link;

public record GetLinkDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<LinkDto>>;
