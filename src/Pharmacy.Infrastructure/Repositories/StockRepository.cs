using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Stock entity
/// </summary>
public class StockRepository : BaseRepository<Stock>, IStockRepository
{
    public StockRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<Stock?> GetByProductAndBranchAsync(Guid productId, Guid branchId, string? batchNumber = null, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Product)
            .Include(s => s.Branch)
            .Where(s => s.ProductId == productId && s.BranchId == branchId && s.BatchNumber == batchNumber && !s.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Stock>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Product)
            .Include(s => s.Branch)
            .Where(s => s.BranchId == branchId && !s.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Stock>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Product)
            .Include(s => s.Branch)
            .Where(s => s.ProductId == productId && !s.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<Stock> UpdateQuantityAsync(Guid productId, Guid branchId, decimal quantityChange, string? batchNumber = null, DateTime? expiryDate = null, CancellationToken cancellationToken = default)
    {
        var stock = await GetByProductAndBranchAsync(productId, branchId, batchNumber, cancellationToken);

        if (stock == null)
        {
            stock = new Stock
            {
                ProductId = productId,
                BranchId = branchId,
                BatchNumber = batchNumber,
                ExpiryDate = expiryDate,
                Quantity = Math.Max(0, quantityChange),
                ReservedQuantity = 0
            };
            await AddAsync(stock, cancellationToken);
        }
        else
        {
            stock.Quantity = (stock.Quantity ?? 0) + quantityChange;
            if (stock.Quantity < 0) stock.Quantity = 0;
            // Update expiry date if provided and not already set
            if (expiryDate.HasValue && !stock.ExpiryDate.HasValue)
                stock.ExpiryDate = expiryDate;
            await UpdateAsync(stock, cancellationToken);
        }

        return stock;
    }

    public async Task<bool> ReserveStockAsync(Guid productId, Guid branchId, decimal quantity, string? batchNumber = null, CancellationToken cancellationToken = default)
    {
        var stock = await GetByProductAndBranchAsync(productId, branchId, batchNumber, cancellationToken);

        if (stock == null || stock.AvailableQuantity < quantity)
        {
            return false;
        }

        stock.ReservedQuantity = (stock.ReservedQuantity ?? 0) + quantity;
        await UpdateAsync(stock, cancellationToken);
        return true;
    }

    public async Task<bool> ReleaseReservedStockAsync(Guid productId, Guid branchId, decimal quantity, string? batchNumber = null, CancellationToken cancellationToken = default)
    {
        var stock = await GetByProductAndBranchAsync(productId, branchId, batchNumber, cancellationToken);

        if (stock == null)
        {
            return false;
        }

        stock.ReservedQuantity = Math.Max(0, (stock.ReservedQuantity ?? 0) - quantity);
        await UpdateAsync(stock, cancellationToken);
        return true;
    }

    public async Task<bool> HasSufficientStockAsync(Guid productId, Guid branchId, decimal quantity, string? batchNumber = null, CancellationToken cancellationToken = default)
    {
        if (batchNumber != null)
        {
            // Check specific batch
            var stock = await GetByProductAndBranchAsync(productId, branchId, batchNumber, cancellationToken);
            return stock != null && stock.AvailableQuantity >= quantity;
        }

        // Check total across all batches for this product+branch
        var totalAvailable = await _dbSet
            .Where(s => s.ProductId == productId && s.BatchNumber == batchNumber && s.BranchId == branchId && !s.IsDeleted)
            .SumAsync(s => (s.Quantity ?? 0) - (s.ReservedQuantity ?? 0), cancellationToken);

        return totalAvailable >= quantity;
    }
}
