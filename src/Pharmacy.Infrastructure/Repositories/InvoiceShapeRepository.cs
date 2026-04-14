using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class InvoiceShapeRepository : BaseRepository<InvoiceShape>, IInvoiceShapeRepository
{
    public InvoiceShapeRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<InvoiceShape>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.Branch)
            .Where(x => x.BranchId == branchId && !x.IsDeleted)
            .OrderBy(x => x.ShapeName)
            .ToListAsync(cancellationToken);
    }

    public async Task<InvoiceShape?> GetByNameAsync(string shapeName, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.ShapeName == shapeName && !x.IsDeleted, cancellationToken);
    }

    public async Task<bool> NameExistsAsync(string shapeName, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => x.ShapeName == shapeName && !x.IsDeleted && (excludeId == null || x.Oid != excludeId), cancellationToken);
    }
}
