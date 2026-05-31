using MediatR;

namespace Pharmacy.Application.Commands.RoleLink;

public record DeleteRoleLinkCommand(Guid Id) : IRequest<bool>;
