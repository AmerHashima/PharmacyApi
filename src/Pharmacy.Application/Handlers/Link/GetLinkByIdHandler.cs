using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Link;
using Pharmacy.Application.Queries.Link;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Link;

public class GetLinkByIdHandler : IRequestHandler<GetLinkByIdQuery, LinkDto?>
{
    private readonly ILinkRepository _linkRepository;
    private readonly IMapper _mapper;

    public GetLinkByIdHandler(ILinkRepository linkRepository, IMapper mapper)
    {
        _linkRepository = linkRepository;
        _mapper = mapper;
    }

    public async Task<LinkDto?> Handle(GetLinkByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _linkRepository.GetWithParametersAsync(request.Id, cancellationToken);
        return entity is null ? null : _mapper.Map<LinkDto>(entity);
    }
}
