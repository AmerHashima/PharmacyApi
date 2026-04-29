using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Link;
using Pharmacy.Application.DTOs.Link;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Link;

public class CreateLinkHandler : IRequestHandler<CreateLinkCommand, LinkDto>
{
    private readonly ILinkRepository _linkRepository;
    private readonly IMapper _mapper;

    public CreateLinkHandler(ILinkRepository linkRepository, IMapper mapper)
    {
        _linkRepository = linkRepository;
        _mapper = mapper;
    }

    public async Task<LinkDto> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        var link = _mapper.Map<Domain.Entities.Link>(request.Link);
        link.CreatedAt = DateTime.UtcNow;

        var parameters = _mapper.Map<List<ReportParameter>>(request.Link.ReportParameters);
        foreach (var param in parameters)
        {
            param.LinksOid = link.Oid;
            param.CreatedAt = DateTime.UtcNow;
        }

        await _linkRepository.InsertMasterDetailAsync(link, parameters, cancellationToken);

        var created = await _linkRepository.GetWithParametersAsync(link.Oid, cancellationToken);
        return _mapper.Map<LinkDto>(created);
    }
}
