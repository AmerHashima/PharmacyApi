using Pharmacy.Application.DTOs.Role;
using MediatR;

namespace Pharmacy.Application.Queries.Role;

public record GetRoleByIdQuery(Guid Id) : IRequest<RoleDto?>;