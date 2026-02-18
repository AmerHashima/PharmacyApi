using MediatR;

namespace Pharmacy.Application.Commands.Role;

public record DeleteRoleCommand(Guid Id) : IRequest<bool>;