using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class ProductUnitRepository : BaseRepository<ProductUnit>, IProductUnitRepository
{
    public ProductUnitRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProductUnit>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(u => u.Product)
            .Include(u => u.PackageType)
            .Where(u => u.ProductId == productId && !u.IsDeleted)
            .OrderBy(u => u.ConversionFactor)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductUnit?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(u => u.Product)
            .Include(u => u.PackageType)
            .Where(u => u.Oid == id && !u.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProductUnit?> GetByProductAndPackageTypeAsync(Guid productId, Guid packageTypeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(u => u.Product)
            .Include(u => u.PackageType)
            .Where(u => u.ProductId == productId && u.PackageTypeId == packageTypeId && !u.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
