using AutoMapper;
using Pharmacy.Application.Commands.Role;
using Pharmacy.Application.DTOs.Role;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Role;

public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
{
    private readonly IRoleRepository _repository;
    private readonly IMapper _mapper;

    public UpdateRoleHandler(IRoleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var existingRole = await _repository.GetByIdAsync(request.Role.Oid, cancellationToken);
        if (existingRole == null)
        {
            throw new KeyNotFoundException($"Role with ID {request.Role.Oid} not found");
        }

        // Check if role name is unique (excluding current role)
        if (await _repository.RoleNameExistsAsync(request.Role.Name, request.Role.Oid, cancellationToken))
        {
            throw new InvalidOperationException($"Role name '{request.Role.Name}' is already in use");
        }

        // Update properties
        existingRole.Name = request.Role.Name;
        existingRole.Description = request.Role.Description;
        existingRole.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(existingRole, cancellationToken);
        return _mapper.Map<RoleDto>(existingRole);
    }
}