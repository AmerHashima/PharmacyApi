using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for StockTransactionDetail entity
/// </summary>
public class StockTransactionDetailRepository : BaseRepository<StockTransactionDetail>, IStockTransactionDetailRepository
{
    public StockTransactionDetailRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<StockTransactionDetail>> GetByTransactionIdAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Product)
            .Where(d => d.StockTransactionId == transactionId && !d.IsDeleted)
            .OrderBy(d => d.LineNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransactionDetail>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.StockTransaction)
            .Where(d => d.ProductId == productId && !d.IsDeleted)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransactionDetail>> GetByBatchNumberAsync(string batchNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Product)
            .Include(d => d.StockTransaction)
            .Where(d => d.BatchNumber == batchNumber && !d.IsDeleted)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransactionDetail>> GetByGTINAsync(string gtin, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Product)
            .Include(d => d.StockTransaction)
            .Where(d => d.Gtin == gtin && !d.IsDeleted)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteByTransactionIdAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        var details = await _dbSet
            .Where(d => d.StockTransactionId == transactionId)
            .ToListAsync(cancellationToken);

        foreach (var detail in details)
        {
            detail.IsDeleted = true;
            detail.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
