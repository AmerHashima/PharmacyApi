using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class IntegrationProviderRepository : BaseRepository<IntegrationProvider>, IIntegrationProviderRepository
{
    public IntegrationProviderRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IntegrationProvider?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.Name == name && !x.IsDeleted);
    }

    public async Task<IEnumerable<IntegrationProvider>> GetActiveProvidersAsync()
    {
        return await _dbSet
            .Where(x => x.Status == 1 && !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(string name, Guid? excludeId = null)
    {
        var query = _dbSet.Where(x => x.Name == name && !x.IsDeleted);
        
        if (excludeId.HasValue)
            query = query.Where(x => x.Oid != excludeId.Value);

        return await query.AnyAsync();
    }
}