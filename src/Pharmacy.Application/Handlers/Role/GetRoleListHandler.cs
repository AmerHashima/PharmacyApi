using AutoMapper;
using Pharmacy.Application.DTOs.Role;
using Pharmacy.Application.Queries.Role;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Role;

public class GetRoleListHandler : IRequestHandler<GetRoleListQuery, IEnumerable<RoleDto>>
{
    private readonly IRoleRepository _repository;
    private readonly IMapper _mapper;

    public GetRoleListHandler(IRoleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
    {
        var roles = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }
}