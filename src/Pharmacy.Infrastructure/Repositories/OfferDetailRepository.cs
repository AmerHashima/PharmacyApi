using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class OfferDetailRepository : BaseRepository<OfferDetail>, IOfferDetailRepository
{
    public OfferDetailRepository(PharmacyDbContext context) : base(context) { }

    public override async Task<OfferDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.OfferMaster)
                .ThenInclude(m => m.OfferType)
            .Include(d => d.Product)
            .Include(d => d.FreeProduct)
            .Where(d => d.Oid == id && !d.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<OfferDetail>> GetByOfferMasterIdAsync(Guid offerMasterId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Product)
            .Include(d => d.FreeProduct)
            .Where(d => d.OfferMasterId == offerMasterId && !d.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteByOfferMasterIdAsync(Guid offerMasterId, CancellationToken cancellationToken = default)
    {
        var details = await _dbSet
            .Where(d => d.OfferMasterId == offerMasterId)
            .ToListAsync(cancellationToken);

        foreach (var detail in details)
        {
            detail.IsDeleted = true;
            detail.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
