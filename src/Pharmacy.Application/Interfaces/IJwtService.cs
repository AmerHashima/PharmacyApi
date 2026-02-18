using Pharmacy.Domain.Entities;
using System.Security.Claims;

namespace Pharmacy.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(SystemUser user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    Task<bool> IsTokenBlacklistedAsync(string token);
    Task BlacklistTokenAsync(string token, DateTime expiration);
}