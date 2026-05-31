using MediatR;
using Pharmacy.Application.DTOs.RoleLink;

namespace Pharmacy.Application.Queries.RoleLink;

public record GetRoleLinkByIdQuery(Guid Id) : IRequest<RoleLinkDto?>;
