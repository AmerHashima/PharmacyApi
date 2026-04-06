using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Store entity
/// </summary>
public class StoreRepository : BaseRepository<Store>, IStoreRepository
{
    public StoreRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Store>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Branch)
            .Where(s => s.BranchId == branchId && !s.IsDeleted)
            .OrderBy(s => s.StoreName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Store?> GetByCodeAsync(string storeCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Branch)
            .FirstOrDefaultAsync(s => s.StoreCode == storeCode && !s.IsDeleted, cancellationToken);
    }

    public async Task<bool> CodeExistsAsync(string storeCode, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(s =>
            s.StoreCode == storeCode &&
            !s.IsDeleted &&
            (excludeId == null || s.Oid != excludeId), cancellationToken);
    }
}
