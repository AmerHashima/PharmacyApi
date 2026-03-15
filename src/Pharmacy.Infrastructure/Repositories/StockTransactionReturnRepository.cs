using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for StockTransactionReturn entity
/// </summary>
public class StockTransactionReturnRepository : BaseRepository<StockTransactionReturn>, IStockTransactionReturnRepository
{
    public StockTransactionReturnRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<StockTransactionReturn>> GetByTypeAsync(Guid transactionTypeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Details)
                .ThenInclude(d => d.Product)
            .Include(t => t.FromBranch)
            .Include(t => t.ToBranch)
            .Include(t => t.TransactionType)
            .Include(t => t.Supplier)
            .Include(t => t.ReturnInvoice)
            .Where(t => t.TransactionTypeId == transactionTypeId && !t.IsDeleted)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransactionReturn>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Details)
                .ThenInclude(d => d.Product)
            .Include(t => t.FromBranch)
            .Include(t => t.ToBranch)
            .Include(t => t.TransactionType)
            .Include(t => t.ReturnInvoice)
            .Where(t => (t.FromBranchId == branchId || t.ToBranchId == branchId) && !t.IsDeleted)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransactionReturn>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? branchId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(t => t.Details)
                .ThenInclude(d => d.Product)
            .Include(t => t.FromBranch)
            .Include(t => t.ToBranch)
            .Include(t => t.TransactionType)
            .Include(t => t.Supplier)
            .Include(t => t.ReturnInvoice)
            .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate && !t.IsDeleted);

        if (branchId.HasValue)
        {
            query = query.Where(t => t.FromBranchId == branchId || t.ToBranchId == branchId);
        }

        return await query
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockTransactionReturn>> GetByReturnInvoiceIdAsync(Guid returnInvoiceId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Details)
                .ThenInclude(d => d.Product)
            .Include(t => t.FromBranch)
            .Include(t => t.ToBranch)
            .Include(t => t.TransactionType)
            .Include(t => t.Supplier)
            .Where(t => t.ReturnInvoiceId == returnInvoiceId && !t.IsDeleted)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }
}
