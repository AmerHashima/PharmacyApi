using Pharmacy.Application.DTOs.Auth;
using MediatR;

namespace Pharmacy.Application.Commands.Auth;

public class RegisterCommand : IRequest<AuthResponseDto>
{
    public RegisterDto RegisterDto { get; set; } = null!;
}