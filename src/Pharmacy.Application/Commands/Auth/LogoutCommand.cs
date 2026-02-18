using MediatR;

namespace Pharmacy.Application.Commands.Auth;

public class LogoutCommand : IRequest<bool>
{
    public string Token { get; set; } = string.Empty;
}