using MediatR;
using Pharmacy.Application.DTOs.RoleLink;

namespace Pharmacy.Application.Commands.RoleLink;

public record UpdateRoleLinkCommand(UpdateRoleLinkDto Dto) : IRequest<RoleLinkDto>;
