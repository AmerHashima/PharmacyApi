using MediatR;
using Pharmacy.Application.DTOs.RoleLink;

namespace Pharmacy.Application.Queries.RoleLink;

public record GetRoleLinksByLinkIdQuery(Guid LinkId) : IRequest<IEnumerable<RoleLinkDto>>;
