using Pharmacy.Application.DTOs.SystemUserSpace;

namespace Pharmacy.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public SystemUserDto User { get; set; } = null!;
}