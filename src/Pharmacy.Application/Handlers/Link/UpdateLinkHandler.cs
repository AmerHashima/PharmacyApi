using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Link;
using Pharmacy.Application.DTOs.Link;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Link;

public class UpdateLinkHandler : IRequestHandler<UpdateLinkCommand, LinkDto>
{
    private readonly ILinkRepository _linkRepository;
    private readonly IReportParameterRepository _parameterRepository;
    private readonly IMapper _mapper;

    public UpdateLinkHandler(ILinkRepository linkRepository, IReportParameterRepository parameterRepository, IMapper mapper)
    {
        _linkRepository = linkRepository;
        _parameterRepository = parameterRepository;
        _mapper = mapper;
    }

    public async Task<LinkDto> Handle(UpdateLinkCommand request, CancellationToken cancellationToken)
    {
        var entity = await _linkRepository.GetWithParametersAsync(request.Link.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"Link with ID '{request.Link.Oid}' not found");

        _mapper.Map(request.Link, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await _parameterRepository.DeleteByLinkIdAsync(entity.Oid, cancellationToken);

        var newParameters = _mapper.Map<List<ReportParameter>>(request.Link.ReportParameters);
        foreach (var param in newParameters)
        {
            param.LinksOid = entity.Oid;
            param.CreatedAt = DateTime.UtcNow;
        }

        await _linkRepository.UpdateMasterDetailAsync(entity, newParameters,
            p => p.LinksOid, cancellationToken);

        var updated = await _linkRepository.GetWithParametersAsync(entity.Oid, cancellationToken);
        return _mapper.Map<LinkDto>(updated);
    }
}
