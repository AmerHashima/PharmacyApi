using AutoMapper;
using Pharmacy.Application.DTOs.Role;
using Pharmacy.Application.Queries.Role;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Role;

public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly IRoleRepository _repository;
    private readonly IMapper _mapper;

    public GetRoleByIdHandler(IRoleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RoleDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return role == null ? null : _mapper.Map<RoleDto>(role);
    }
}