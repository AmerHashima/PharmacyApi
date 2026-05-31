using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.RoleLink;
using Pharmacy.Application.Queries.RoleLink;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.RoleLink;

public class GetRoleLinkByIdHandler : IRequestHandler<GetRoleLinkByIdQuery, RoleLinkDto?>
{
    private readonly IRoleLinkRepository _repository;
    private readonly IMapper _mapper;

    public GetRoleLinkByIdHandler(IRoleLinkRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<RoleLinkDto?> Handle(GetRoleLinkByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return entity is null ? null : _mapper.Map<RoleLinkDto>(entity);
    }
}
