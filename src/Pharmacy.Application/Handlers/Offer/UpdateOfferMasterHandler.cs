using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Offer;
using Pharmacy.Application.DTOs.Offer;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Offer;

public class UpdateOfferMasterHandler : IRequestHandler<UpdateOfferMasterCommand, OfferMasterDto>
{
    private readonly IOfferMasterRepository _masterRepository;
    private readonly IOfferDetailRepository _detailRepository;
    private readonly IMapper _mapper;

    public UpdateOfferMasterHandler(IOfferMasterRepository masterRepository, IOfferDetailRepository detailRepository, IMapper mapper)
    {
        _masterRepository = masterRepository;
        _detailRepository = detailRepository;
        _mapper = mapper;
    }

    public async Task<OfferMasterDto> Handle(UpdateOfferMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _masterRepository.GetWithDetailsAsync(request.Offer.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"Offer with ID '{request.Offer.Oid}' not found");

        _mapper.Map(request.Offer, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        // Replace all detail rows
        await _detailRepository.DeleteByOfferMasterIdAsync(entity.Oid, cancellationToken);

        var newDetails = _mapper.Map<List<OfferDetail>>(request.Offer.OfferDetails);
        foreach (var detail in newDetails)
        {
            detail.OfferMasterId = entity.Oid;
            detail.CreatedAt = DateTime.UtcNow;
        }

        await _masterRepository.UpdateMasterDetailAsync(entity, newDetails,
            d => d.OfferMasterId, cancellationToken);

        var updated = await _masterRepository.GetWithDetailsAsync(entity.Oid, cancellationToken);
        return _mapper.Map<OfferMasterDto>(updated);
    }
}
