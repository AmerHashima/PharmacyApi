using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Branch entity
/// </summary>
public class BranchRepository : BaseRepository<Branch>, IBranchRepository
{
    public BranchRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<bool> IsBranchCodeUniqueAsync(string branchCode, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(b => b.BranchCode == branchCode && !b.IsDeleted);
        
        if (excludeId.HasValue)
        {
            query = query.Where(b => b.Oid != excludeId.Value);
        }
        
        return !await query.AnyAsync(cancellationToken);
    }

    public async Task<Branch?> GetByCodeAsync(string branchCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(b => b.BranchCode == branchCode && !b.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Branch?> GetWithUsersAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Users)
            .Where(b => b.Oid == branchId && !b.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Branch?> GetWithStockAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Stocks)
                .ThenInclude(s => s.Product)
            .Where(b => b.Oid == branchId && !b.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
