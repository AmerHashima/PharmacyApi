using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Link;
using Pharmacy.Application.Queries.Link;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Link;

public class GetActiveLinksHandler : IRequestHandler<GetActiveLinksQuery, IEnumerable<LinkDto>>
{
    private readonly ILinkRepository _linkRepository;
    private readonly IMapper _mapper;

    public GetActiveLinksHandler(ILinkRepository linkRepository, IMapper mapper)
    {
        _linkRepository = linkRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LinkDto>> Handle(GetActiveLinksQuery request, CancellationToken cancellationToken)
    {
        var entities = await _linkRepository.GetActiveLinksAsync(cancellationToken);
        return _mapper.Map<IEnumerable<LinkDto>>(entities);
    }
}
