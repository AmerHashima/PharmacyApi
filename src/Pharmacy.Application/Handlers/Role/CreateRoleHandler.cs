using AutoMapper;
using Pharmacy.Application.Commands.Role;
using Pharmacy.Application.DTOs.Role;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Role;

public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, RoleDto>
{
    private readonly IRoleRepository _repository;
    private readonly IMapper _mapper;

    public CreateRoleHandler(IRoleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        // Check if role name already exists
        if (await _repository.RoleNameExistsAsync(request.Role.Name, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException($"Role with name '{request.Role.Name}' already exists");
        }

        var role = _mapper.Map<Domain.Entities.Role>(request.Role);
        var createdRole = await _repository.AddAsync(role, cancellationToken);

        return _mapper.Map<RoleDto>(createdRole);
    }
}