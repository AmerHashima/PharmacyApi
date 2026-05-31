using MediatR;
using Pharmacy.Application.DTOs.RoleLink;

namespace Pharmacy.Application.Commands.RoleLink;

/// <summary>Replace all link-permissions for a role atomically.</summary>
public record SetRoleLinksCommand(SetRoleLinksDto Dto) : IRequest<IEnumerable<RoleLinkDto>>;
