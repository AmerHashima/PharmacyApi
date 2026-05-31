using MediatR;
using Pharmacy.Application.DTOs.RoleLink;

namespace Pharmacy.Application.Queries.RoleLink;

public record GetRoleLinksByRoleIdQuery(Guid RoleId) : IRequest<IEnumerable<RoleLinkDto>>;
