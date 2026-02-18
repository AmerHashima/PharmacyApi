using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Stakeholder entity
/// </summary>
public class StakeholderRepository : BaseRepository<Stakeholder>, IStakeholderRepository
{
    public StakeholderRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Stakeholder>> GetByTypeAsync(Guid stakeholderTypeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.StakeholderType)
            .Where(s => s.StakeholderTypeId == stakeholderTypeId && !s.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<Stakeholder?> GetWithBranchesAsync(Guid stakeholderId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.StakeholderBranches)
                .ThenInclude(sb => sb.Branch)
            .Where(s => s.Oid == stakeholderId && !s.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Stakeholder>> GetActiveSuppliersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.StakeholderType)
            .Where(s => s.IsActive && !s.IsDeleted && s.StakeholderType != null && s.StakeholderType.ValueCode == "SUPPLIER")
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsGLNUniqueAsync(string gln, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(s => s.GLN == gln && !s.IsDeleted);
        
        if (excludeId.HasValue)
        {
            query = query.Where(s => s.Oid != excludeId.Value);
        }
        
        return !await query.AnyAsync(cancellationToken);
    }
}
