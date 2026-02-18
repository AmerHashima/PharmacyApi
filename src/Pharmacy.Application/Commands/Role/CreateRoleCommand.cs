using Pharmacy.Application.DTOs.Role;
using MediatR;

namespace Pharmacy.Application.Commands.Role;

public record CreateRoleCommand(CreateRoleDto Role) : IRequest<RoleDto>;