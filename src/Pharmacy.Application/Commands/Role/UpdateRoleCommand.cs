using Pharmacy.Application.DTOs.Role;
using MediatR;

namespace Pharmacy.Application.Commands.Role;

public record UpdateRoleCommand(UpdateRoleDto Role) : IRequest<RoleDto>;