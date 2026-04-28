using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Offer;
using Pharmacy.Application.Queries.Offer;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Offer;

public class GetActiveOffersHandler : IRequestHandler<GetActiveOffersQuery, IEnumerable<OfferMasterDto>>
{
    private readonly IOfferMasterRepository _masterRepository;
    private readonly IMapper _mapper;

    public GetActiveOffersHandler(IOfferMasterRepository masterRepository, IMapper mapper)
    {
        _masterRepository = masterRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OfferMasterDto>> Handle(GetActiveOffersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _masterRepository.GetActiveOffersAsync(cancellationToken);
        return _mapper.Map<IEnumerable<OfferMasterDto>>(entities);
    }
}
