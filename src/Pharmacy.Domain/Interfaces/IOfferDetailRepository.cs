using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IOfferDetailRepository : IBaseRepository<OfferDetail>
{
    Task<IEnumerable<OfferDetail>> GetByOfferMasterIdAsync(Guid offerMasterId, CancellationToken cancellationToken = default);
    Task DeleteByOfferMasterIdAsync(Guid offerMasterId, CancellationToken cancellationToken = default);
}
