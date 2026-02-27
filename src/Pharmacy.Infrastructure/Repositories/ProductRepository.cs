using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Product entity
/// </summary>
public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<bool> IsGTINUniqueAsync(string gtin, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(p => p.GTIN == gtin && !p.IsDeleted);
        
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Oid != excludeId.Value);
        }
        
        return !await query.AnyAsync(cancellationToken);
    }

    public async Task<Product?> GetByGTINAsync(string gtin, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.ProductType)
            .Where(p => '0'+p.GTIN == gtin && !p.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetByTypeAsync(Guid productTypeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.ProductType)
            .Where(p => p.ProductTypeId == productTypeId && !p.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetWithStockAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Stocks)
                .ThenInclude(s => s.Branch)
            .Where(p => p.Oid == productId && !p.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Product?> GetWithBatchesAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Batches)
            .Where(p => p.Oid == productId && !p.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        return await _dbSet
            .Include(p => p.ProductType)
            .Where(p => !p.IsDeleted && 
                (p.DrugName.ToLower().Contains(lowerSearchTerm) || 
                 (p.GenericName != null && p.GenericName.ToLower().Contains(lowerSearchTerm)) ||
                 (p.GTIN != null && p.GTIN.Contains(searchTerm))))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Stocks)
            .Where(p => !p.IsDeleted && p.MinStockLevel.HasValue &&
                p.Stocks.Any(s => s.BranchId == branchId && !s.IsDeleted && s.Quantity < p.MinStockLevel))
            .ToListAsync(cancellationToken);
    }
}
