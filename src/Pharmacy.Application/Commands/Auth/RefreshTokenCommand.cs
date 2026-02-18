using Pharmacy.Application.DTOs.Auth;
using MediatR;

namespace Pharmacy.Application.Commands.Auth;

public class RefreshTokenCommand : IRequest<AuthResponseDto>
{
    public RefreshTokenDto RefreshTokenDto { get; set; } = null!;
}