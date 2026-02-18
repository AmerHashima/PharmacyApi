using Pharmacy.Application.DTOs.Role;
using MediatR;

namespace Pharmacy.Application.Queries.Role;

public record GetRoleListQuery() : IRequest<IEnumerable<RoleDto>>;