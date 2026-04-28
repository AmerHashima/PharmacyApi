using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class OfferMasterRepository : BaseRepository<OfferMaster>, IOfferMasterRepository
{
    public OfferMasterRepository(PharmacyDbContext context) : base(context) { }

    public async Task<OfferMaster?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OfferType)
            .Include(o => o.Branch)
            .Include(o => o.OfferDetails)
                .ThenInclude(d => d.Product)
            .Include(o => o.OfferDetails)
                .ThenInclude(d => d.FreeProduct)
            .Where(o => o.Oid == id && !o.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<OfferMaster>> GetActiveOffersAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet
            .Include(o => o.OfferType)
            .Include(o => o.OfferDetails)
                .ThenInclude(d => d.Product)
            .Where(o => !o.IsDeleted
                && o.StartDate.Date <= today
                && (o.EndDate == null || o.EndDate.Value.Date >= today))
            .OrderBy(o => o.OfferNameEn)
            .ToListAsync(cancellationToken);
    }
}
