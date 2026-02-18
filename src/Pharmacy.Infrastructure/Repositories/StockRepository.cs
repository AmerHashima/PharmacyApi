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

    public async Task<Stock?> GetByProductAndBranchAsync(Guid productId, Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Product)
            .Include(s => s.Branch)
            .Where(s => s.ProductId == productId && s.BranchId == branchId && !s.IsDeleted)
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

    public async Task<Stock> UpdateQuantityAsync(Guid productId, Guid branchId, decimal quantityChange, CancellationToken cancellationToken = default)
    {
        var stock = await GetByProductAndBranchAsync(productId, branchId, cancellationToken);
        
        if (stock == null)
        {
            // Create new stock record if it doesn't exist
            stock = new Stock
            {
                ProductId = productId,
                BranchId = branchId,
                Quantity = Math.Max(0, quantityChange),
                ReservedQuantity = 0
            };
            await AddAsync(stock, cancellationToken);
        }
        else
        {
            stock.Quantity = (stock.Quantity ?? 0) + quantityChange;
            if (stock.Quantity < 0) stock.Quantity = 0;
            await UpdateAsync(stock, cancellationToken);
        }
        
        return stock;
    }

    public async Task<bool> ReserveStockAsync(Guid productId, Guid branchId, decimal quantity, CancellationToken cancellationToken = default)
    {
        var stock = await GetByProductAndBranchAsync(productId, branchId, cancellationToken);
        
        if (stock == null || stock.AvailableQuantity < quantity)
        {
            return false;
        }
        
        stock.ReservedQuantity = (stock.ReservedQuantity ?? 0) + quantity;
        await UpdateAsync(stock, cancellationToken);
        return true;
    }

    public async Task<bool> ReleaseReservedStockAsync(Guid productId, Guid branchId, decimal quantity, CancellationToken cancellationToken = default)
    {
        var stock = await GetByProductAndBranchAsync(productId, branchId, cancellationToken);
        
        if (stock == null)
        {
            return false;
        }
        
        stock.ReservedQuantity = Math.Max(0, (stock.ReservedQuantity ?? 0) - quantity);
        await UpdateAsync(stock, cancellationToken);
        return true;
    }

    public async Task<bool> HasSufficientStockAsync(Guid productId, Guid branchId, decimal quantity, CancellationToken cancellationToken = default)
    {
        var stock = await GetByProductAndBranchAsync(productId, branchId, cancellationToken);
        return stock != null && stock.AvailableQuantity >= quantity;
    }
}
