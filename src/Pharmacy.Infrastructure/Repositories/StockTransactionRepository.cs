using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for StockTransaction entity
/// </summary>
public class StockTransactionRepository : BaseRepository<StockTransaction>, IStockTransactionRepository
{
    public StockTransactionRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<StockTransaction>> GetByTypeAsync(Guid transactionTypeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Product)
            .Include(t => t.FromBranch)
            .Include(t => t.ToBranch)
            .Include(t => t.TransactionType)
            .Include(t => t.Supplier)
            .Where(t => t.TransactionTypeId == transactionTypeId && !t.IsDeleted)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransaction>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Product)
            .Include(t => t.FromBranch)
            .Include(t => t.ToBranch)
            .Include(t => t.TransactionType)
            .Where(t => t.ProductId == productId && !t.IsDeleted)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransaction>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Product)
            .Include(t => t.FromBranch)
            .Include(t => t.ToBranch)
            .Include(t => t.TransactionType)
            .Where(t => (t.FromBranchId == branchId || t.ToBranchId == branchId) && !t.IsDeleted)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? branchId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(t => t.Product)
            .Include(t => t.FromBranch)
            .Include(t => t.ToBranch)
            .Include(t => t.TransactionType)
            .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate && !t.IsDeleted);
        
        if (branchId.HasValue)
        {
            query = query.Where(t => t.FromBranchId == branchId.Value || t.ToBranchId == branchId.Value);
        }
        
        return await query
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<StockTransaction?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Product)
            .Include(t => t.FromBranch)
            .Include(t => t.ToBranch)
            .Where(t => t.ReferenceNumber == referenceNumber && !t.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string> GenerateReferenceNumberAsync(string prefix, CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.ToString("yyyyMMdd");
        var lastTransaction = await _dbSet
            .Where(t => t.ReferenceNumber != null && t.ReferenceNumber.StartsWith($"{prefix}-{today}"))
            .OrderByDescending(t => t.ReferenceNumber)
            .FirstOrDefaultAsync(cancellationToken);
        
        int sequence = 1;
        if (lastTransaction?.ReferenceNumber != null)
        {
            var parts = lastTransaction.ReferenceNumber.Split('-');
            if (parts.Length >= 3 && int.TryParse(parts[^1], out int lastSequence))
            {
                sequence = lastSequence + 1;
            }
        }
        
        return $"{prefix}-{today}-{sequence:D4}";
    }
}
