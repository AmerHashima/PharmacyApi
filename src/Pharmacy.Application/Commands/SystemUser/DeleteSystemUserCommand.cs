using MediatR;

namespace Pharmacy.Application.Commands.SystemUserSpace;

public record DeleteSystemUserCommand(Guid Id) : IRequest<bool>;