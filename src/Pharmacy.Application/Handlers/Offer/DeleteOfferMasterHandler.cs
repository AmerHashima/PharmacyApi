using MediatR;
using Pharmacy.Application.Commands.Offer;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Offer;

public class DeleteOfferMasterHandler : IRequestHandler<DeleteOfferMasterCommand, bool>
{
    private readonly IOfferMasterRepository _masterRepository;
    private readonly IOfferDetailRepository _detailRepository;

    public DeleteOfferMasterHandler(IOfferMasterRepository masterRepository, IOfferDetailRepository detailRepository)
    {
        _masterRepository = masterRepository;
        _detailRepository = detailRepository;
    }

    public async Task<bool> Handle(DeleteOfferMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _masterRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;

        await _detailRepository.DeleteByOfferMasterIdAsync(request.Id, cancellationToken);
        await _masterRepository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
