using Pharmacy.Application.DTOs.SystemUserSpace;
using MediatR;

namespace Pharmacy.Application.Commands.SystemUserSpace;

public record UpdateSystemUserCommand(UpdateSystemUserDto SystemUser) : IRequest<SystemUserDto>;