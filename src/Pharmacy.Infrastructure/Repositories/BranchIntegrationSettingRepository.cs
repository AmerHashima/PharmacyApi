using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class BranchIntegrationSettingRepository : BaseRepository<BranchIntegrationSetting>, IBranchIntegrationSettingRepository
{
    public BranchIntegrationSettingRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BranchIntegrationSetting>> GetByBranchIdAsync(Guid branchId)
    {
        return await _dbSet
            .Include(x => x.IntegrationProvider)
            .Include(x => x.Branch)
            .Where(x => x.BranchId == branchId && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<BranchIntegrationSetting>> GetByProviderIdAsync(Guid providerId)
    {
        return await _dbSet
            .Include(x => x.IntegrationProvider)
            .Include(x => x.Branch)
            .Where(x => x.IntegrationProviderId == providerId && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<BranchIntegrationSetting?> GetByBranchAndProviderAsync(Guid branchId, Guid providerId)
    {
        return await _dbSet
            .Include(x => x.IntegrationProvider)
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.BranchId == branchId && x.IntegrationProviderId == providerId && !x.IsDeleted);
    }

    public async Task<bool> ExistsAsync(Guid branchId, Guid providerId, Guid? excludeId = null)
    {
        var query = _dbSet.Where(x => x.BranchId == branchId && x.IntegrationProviderId == providerId && !x.IsDeleted);
        
        if (excludeId.HasValue)
            query = query.Where(x => x.Oid != excludeId.Value);

        return await query.AnyAsync();
    }
}