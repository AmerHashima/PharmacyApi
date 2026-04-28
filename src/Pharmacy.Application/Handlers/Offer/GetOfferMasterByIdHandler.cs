using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Offer;
using Pharmacy.Application.Queries.Offer;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Offer;

public class GetOfferMasterByIdHandler : IRequestHandler<GetOfferMasterByIdQuery, OfferMasterDto?>
{
    private readonly IOfferMasterRepository _masterRepository;
    private readonly IMapper _mapper;

    public GetOfferMasterByIdHandler(IOfferMasterRepository masterRepository, IMapper mapper)
    {
        _masterRepository = masterRepository;
        _mapper = mapper;
    }

    public async Task<OfferMasterDto?> Handle(GetOfferMasterByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _masterRepository.GetWithDetailsAsync(request.Id, cancellationToken);
        return entity is null ? null : _mapper.Map<OfferMasterDto>(entity);
    }
}
