using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for StockTransactionReturnDetail entity
/// </summary>
public class StockTransactionReturnDetailRepository : BaseRepository<StockTransactionReturnDetail>, IStockTransactionReturnDetailRepository
{
    public StockTransactionReturnDetailRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<StockTransactionReturnDetail>> GetByTransactionReturnIdAsync(Guid transactionReturnId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Product)
            .Where(d => d.StockTransactionReturnId == transactionReturnId && !d.IsDeleted)
            .OrderBy(d => d.LineNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransactionReturnDetail>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.StockTransactionReturn)
            .Where(d => d.ProductId == productId && !d.IsDeleted)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransactionReturnDetail>> GetByBatchNumberAsync(string batchNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Product)
            .Include(d => d.StockTransactionReturn)
            .Where(d => d.BatchNumber == batchNumber && !d.IsDeleted)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
