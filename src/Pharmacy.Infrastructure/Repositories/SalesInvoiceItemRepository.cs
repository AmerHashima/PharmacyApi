using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for SalesInvoiceItem entity
/// </summary>
public class SalesInvoiceItemRepository : BaseRepository<SalesInvoiceItem>, ISalesInvoiceItemRepository
{
    public SalesInvoiceItemRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SalesInvoiceItem>> GetByInvoiceAsync(Guid invoiceId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Product)
            .Where(i => i.InvoiceId == invoiceId && !i.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SalesInvoiceItem>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Invoice)
            .Include(i => i.Product)
            .Where(i => i.ProductId == productId && !i.IsDeleted)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<(Guid ProductId, string ProductName, decimal TotalQuantity, decimal TotalRevenue)>> GetTopSellingProductsAsync(
        Guid branchId,
        DateTime startDate,
        DateTime endDate,
        int top = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _dbSet
            .Include(i => i.Invoice)
            .Include(i => i.Product)
            .Where(i => i.Invoice.BranchId == branchId &&
                       i.Invoice.InvoiceDate >= startDate &&
                       i.Invoice.InvoiceDate <= endDate &&
                       !i.IsDeleted &&
                       !i.Invoice.IsDeleted)
            .GroupBy(i => new { i.ProductId, i.Product.DrugName })
            .Select(g => new
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.DrugName,
                TotalQuantity = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.TotalPrice ?? 0)
            })
            .OrderByDescending(x => x.TotalRevenue)
            .Take(top)
            .ToListAsync(cancellationToken);

        return result.Select(x => (x.ProductId, x.ProductName, x.TotalQuantity, x.TotalRevenue));
    }
}
