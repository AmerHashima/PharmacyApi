using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Role;
using MediatR;

namespace Pharmacy.Application.Queries.Role;

public record GetRoleDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<RoleDto>>;