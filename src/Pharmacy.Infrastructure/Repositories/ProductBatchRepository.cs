using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for ProductBatch entity
/// </summary>
public class ProductBatchRepository : BaseRepository<ProductBatch>, IProductBatchRepository
{
    public ProductBatchRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProductBatch>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Product)
            .Include(b => b.Branch)
            .Include(b => b.BatchStatus)
            .Where(b => b.ProductId == productId && !b.IsDeleted)
            .OrderBy(b => b.ExpiryDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductBatch>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Product)
            .Include(b => b.BatchStatus)
            .Where(b => b.BranchId == branchId && !b.IsDeleted)
            .OrderBy(b => b.ExpiryDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductBatch?> GetByBatchNumberAsync(Guid productId, string batchNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Product)
            .Include(b => b.Branch)
            .Where(b => b.ProductId == productId && b.BatchNumber == batchNumber && !b.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductBatch>> GetExpiringBatchesAsync(Guid branchId, int daysUntilExpiry, CancellationToken cancellationToken = default)
    {
        var expiryThreshold = DateTime.UtcNow.AddDays(daysUntilExpiry);
        
        return await _dbSet
            .Include(b => b.Product)
            .Where(b => b.BranchId == branchId && 
                       b.ExpiryDate <= expiryThreshold && 
                       b.ExpiryDate > DateTime.UtcNow &&
                       b.CurrentQuantity > 0 &&
                       !b.IsDeleted)
            .OrderBy(b => b.ExpiryDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductBatch>> GetExpiredBatchesAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Product)
            .Where(b => b.BranchId == branchId && 
                       b.ExpiryDate <= DateTime.UtcNow &&
                       !b.IsDeleted)
            .OrderBy(b => b.ExpiryDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductBatch?> GetOldestAvailableBatchAsync(Guid productId, Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Product)
            .Where(b => b.ProductId == productId && 
                       b.BranchId == branchId &&
                       b.ExpiryDate > DateTime.UtcNow &&
                       b.CurrentQuantity > 0 &&
                       !b.IsDeleted)
            .OrderBy(b => b.ExpiryDate)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
