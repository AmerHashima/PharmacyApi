using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IOfferMasterRepository : IBaseRepository<OfferMaster>
{
    Task<OfferMaster?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OfferMaster>> GetActiveOffersAsync(CancellationToken cancellationToken = default);
}
