 using AutoMapper;
using Pharmacy.Application.Commands.Auth;
using Pharmacy.Application.DTOs.Auth;
using Pharmacy.Application.DTOs.SystemUserSpace;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace Pharmacy.Application.Handlers.Auth;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly ISystemUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public LoginHandler(ISystemUserRepository userRepository, IJwtService jwtService, IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Get user by username
        var user = await _userRepository.GetByUsernameAsync(request.LoginDto.Username, cancellationToken);
        
        if (user == null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        // Check if account is locked
        //if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
        //{
        //    throw new UnauthorizedAccessException($"Account is locked until {user.LockoutEnd}.");
        //}

        // Verify password
        if (!VerifyPassword(request.LoginDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            // Increment failed login count
            user.FailedLoginCount++;
            if (user.FailedLoginCount >= 5)
            {
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(30);
            }
            await _userRepository.UpdateAsync(user, cancellationToken);
            
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        // Reset failed login count and update last login
        user.FailedLoginCount = 0;
        user.LockoutEnd = null;
        user.LastLogin = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user, cancellationToken);

        // Generate tokens
        var token = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var userDto = _mapper.Map<SystemUserDto>(user);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Expires = DateTime.UtcNow.AddMinutes(60), // This should match JWT expiration
            User = userDto
        };
    }

    private static bool VerifyPassword(string password, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
        var computedHash = Convert.ToBase64String(pbkdf2.GetBytes(32));
        return computedHash == hash;
    }
}