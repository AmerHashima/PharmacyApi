using AutoMapper;
using Pharmacy.Application.Commands.SystemUserSpace;
using Pharmacy.Application.DTOs.SystemUserSpace;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.SystemUserSpace;

public class UpdateSystemUserHandler : IRequestHandler<UpdateSystemUserCommand, SystemUserDto>
{
    private readonly ISystemUserRepository _repository;
    private readonly IMapper _mapper;

    public UpdateSystemUserHandler(ISystemUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SystemUserDto> Handle(UpdateSystemUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _repository.GetByIdAsync(request.SystemUser.Oid, cancellationToken);
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"SystemUser with ID {request.SystemUser.Oid} not found.");
        }

        // Check if username is unique (excluding current user)
        if (!await _repository.IsUsernameUniqueAsync(request.SystemUser.Username, request.SystemUser.Oid, cancellationToken))
        {
            throw new InvalidOperationException($"Username '{request.SystemUser.Username}' is already taken.");
        }

        // Check if email is unique (excluding current user, if provided)
        if (!string.IsNullOrEmpty(request.SystemUser.Email))
        {
            if (!await _repository.IsEmailUniqueAsync(request.SystemUser.Email, request.SystemUser.Oid, cancellationToken))
            {
                throw new InvalidOperationException($"Email '{request.SystemUser.Email}' is already in use.");
            }
        }

        // Update properties
        existingUser.Username = request.SystemUser.Username;
        existingUser.Email = request.SystemUser.Email;
        existingUser.Mobile = request.SystemUser.Mobile;
        existingUser.FirstName = request.SystemUser.FirstName;
        existingUser.MiddleName = request.SystemUser.MiddleName;
        existingUser.LastName = request.SystemUser.LastName;
        existingUser.FullName = $"{request.SystemUser.FirstName} {request.SystemUser.MiddleName} {request.SystemUser.LastName}".Trim().Replace("  ", " ");
        existingUser.GenderLookupId = request.SystemUser.GenderLookupId; // Fixed: Use GenderLookupId (Guid)
        existingUser.BirthDate = request.SystemUser.BirthDate;
        existingUser.RoleId = request.SystemUser.RoleId; // Fixed: Use RoleId (Guid) not RoleID (int)
        existingUser.IsActive = request.SystemUser.IsActive;
        existingUser.TwoFactorEnabled = request.SystemUser.TwoFactorEnabled;
        existingUser.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(existingUser, cancellationToken);
        return _mapper.Map<SystemUserDto>(existingUser);
    }
}