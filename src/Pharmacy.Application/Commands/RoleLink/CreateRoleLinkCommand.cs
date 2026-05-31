using MediatR;
using Pharmacy.Application.DTOs.RoleLink;

namespace Pharmacy.Application.Commands.RoleLink;

public record CreateRoleLinkCommand(CreateRoleLinkDto Dto) : IRequest<RoleLinkDto>;
