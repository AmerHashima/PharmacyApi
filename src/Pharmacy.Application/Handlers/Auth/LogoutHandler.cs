using Pharmacy.Application.Commands.Auth;
using Pharmacy.Application.Interfaces;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Pharmacy.Application.Handlers.Auth;

public class LogoutHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IJwtService _jwtService;

    public LogoutHandler(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(request.Token);
            
            await _jwtService.BlacklistTokenAsync(request.Token, token.ValidTo);
            return true;
        }
        catch
        {
            return false;
        }
    }
}