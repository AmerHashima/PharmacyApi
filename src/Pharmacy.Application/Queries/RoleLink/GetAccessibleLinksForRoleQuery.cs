using MediatR;
using Pharmacy.Application.DTOs.RoleLink;

namespace Pharmacy.Application.Queries.RoleLink;

/// <summary>Returns all links with CanRead=true for the given role.</summary>
public record GetAccessibleLinksForRoleQuery(Guid RoleId) : IRequest<IEnumerable<RoleLinkDto>>;
