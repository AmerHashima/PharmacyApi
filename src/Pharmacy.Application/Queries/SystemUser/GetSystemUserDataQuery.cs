using Pharmacy.Application.DTOs.SystemUserSpace;
using Pharmacy.Application.DTOs.Common;
using MediatR;

namespace Pharmacy.Application.Queries.SystemUserSpace;

public record GetSystemUserDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<SystemUserDto>>;