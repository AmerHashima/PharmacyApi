using AutoMapper;
using Pharmacy.Application.Commands.SystemUserSpace;
using Pharmacy.Application.DTOs.SystemUserSpace;
using Pharmacy.Domain.Interfaces;
using MediatR;
using System.Security.Cryptography;

namespace Pharmacy.Application.Handlers.SystemUserSpace;

public class CreateSystemUserHandler : IRequestHandler<CreateSystemUserCommand, SystemUserDto>
{
    private readonly ISystemUserRepository _repository;
    private readonly IMapper _mapper;

    public CreateSystemUserHandler(ISystemUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SystemUserDto> Handle(CreateSystemUserCommand request, CancellationToken cancellationToken)
    {
        // Check if username already exists
        if (!await _repository.IsUsernameUniqueAsync(request.SystemUser.Username, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException($"Username '{request.SystemUser.Username}' already exists");
        }

        // Check if email already exists (if provided)
        if (!string.IsNullOrEmpty(request.SystemUser.Email))
        {
            if (!await _repository.IsEmailUniqueAsync(request.SystemUser.Email, cancellationToken: cancellationToken))
            {
                throw new InvalidOperationException($"Email '{request.SystemUser.Email}' is already in use");
            }
        }

        // Hash password
        var (hashedPassword, salt) = HashPassword(request.SystemUser.Password);

        var user = _mapper.Map<Domain.Entities.SystemUser>(request.SystemUser);
        user.PasswordHash = hashedPassword;
        user.PasswordSalt = salt;
        user.FullName = $"{user.FirstName} {user.MiddleName} {user.LastName}".Trim().Replace("  ", " ");

        var createdUser = await _repository.AddAsync(user, cancellationToken);
        return _mapper.Map<SystemUserDto>(createdUser);
    }

    private static (string hashedPassword, string salt) HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[32];
        rng.GetBytes(saltBytes);
        var salt = Convert.ToBase64String(saltBytes);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
        var hashedPassword = Convert.ToBase64String(pbkdf2.GetBytes(32));

        return (hashedPassword, salt);
    }
}