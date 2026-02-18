using Pharmacy.Application.DTOs.Auth;
using MediatR;

namespace Pharmacy.Application.Commands.Auth;

public class LoginCommand : IRequest<AuthResponseDto>
{
    public LoginDto LoginDto { get; set; } = null!;
}