using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class RsdOperationLogRepository : BaseRepository<RsdOperationLog>, IRsdOperationLogRepository
{
    public RsdOperationLogRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<RsdOperationLog?> GetWithDetailsAsync(Guid logId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Details)
                .ThenInclude(d => d.Product)
            .Include(l => l.Branch)
            .Where(l => l.Oid == logId && !l.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<RsdOperationLog>> GetByBranchIdAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Details)
            .Where(l => l.BranchId == branchId && !l.IsDeleted)
            .OrderByDescending(l => l.RequestedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<RsdOperationLog>> GetByOperationTypeAsync(Guid operationTypeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Details)
            .Where(l => l.OperationTypeId == operationTypeId && !l.IsDeleted)
            .OrderByDescending(l => l.RequestedAt)
            .ToListAsync(cancellationToken);
    }
}
