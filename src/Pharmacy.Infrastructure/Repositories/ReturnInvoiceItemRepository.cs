using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for ReturnInvoiceItem entity
/// </summary>
public class ReturnInvoiceItemRepository : BaseRepository<ReturnInvoiceItem>, IReturnInvoiceItemRepository
{
    public ReturnInvoiceItemRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ReturnInvoiceItem>> GetByReturnInvoiceAsync(Guid returnInvoiceId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Product)
            .Where(i => i.ReturnInvoiceId == returnInvoiceId && !i.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ReturnInvoiceItem>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.ReturnInvoice)
            .Include(i => i.Product)
            .Where(i => i.ProductId == productId && !i.IsDeleted)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
