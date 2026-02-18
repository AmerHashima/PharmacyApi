using Pharmacy.Application.DTOs.SystemUserSpace;
using MediatR;

namespace Pharmacy.Application.Commands.SystemUserSpace;

public record CreateSystemUserCommand(CreateSystemUserDto SystemUser) : IRequest<SystemUserDto>;