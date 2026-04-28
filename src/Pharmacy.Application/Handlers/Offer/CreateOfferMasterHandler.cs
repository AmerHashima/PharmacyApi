using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Offer;
using Pharmacy.Application.DTOs.Offer;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Offer;

public class CreateOfferMasterHandler : IRequestHandler<CreateOfferMasterCommand, OfferMasterDto>
{
    private readonly IOfferMasterRepository _masterRepository;
    private readonly IMapper _mapper;

    public CreateOfferMasterHandler(IOfferMasterRepository masterRepository, IMapper mapper)
    {
        _masterRepository = masterRepository;
        _mapper = mapper;
    }

    public async Task<OfferMasterDto> Handle(CreateOfferMasterCommand request, CancellationToken cancellationToken)
    {
        var master = _mapper.Map<OfferMaster>(request.Offer);
        master.CreatedAt = DateTime.UtcNow;

        var details = _mapper.Map<List<OfferDetail>>(request.Offer.OfferDetails);
        foreach (var detail in details)
        {
            detail.OfferMasterId = master.Oid;
            detail.CreatedAt = DateTime.UtcNow;
        }

        await _masterRepository.InsertMasterDetailAsync(master, details, cancellationToken);

        var created = await _masterRepository.GetWithDetailsAsync(master.Oid, cancellationToken);
        return _mapper.Map<OfferMasterDto>(created);
    }
}
