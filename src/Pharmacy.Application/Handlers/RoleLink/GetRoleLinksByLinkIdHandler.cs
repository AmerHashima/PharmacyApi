using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.RoleLink;
using Pharmacy.Application.Queries.RoleLink;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.RoleLink;

public class GetRoleLinksByLinkIdHandler : IRequestHandler<GetRoleLinksByLinkIdQuery, IEnumerable<RoleLinkDto>>
{
    private readonly IRoleLinkRepository _repository;
    private readonly IMapper _mapper;

    public GetRoleLinksByLinkIdHandler(IRoleLinkRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<IEnumerable<RoleLinkDto>> Handle(GetRoleLinksByLinkIdQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByLinkIdAsync(request.LinkId, cancellationToken);
        return _mapper.Map<IEnumerable<RoleLinkDto>>(entities);
    }
}
