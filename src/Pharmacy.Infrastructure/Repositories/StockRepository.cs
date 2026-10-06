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
            if (quantityChange < 0)
                throw new InvalidOperationException(
                    $"Insufficient stock for product '{productId}' at branch '{branchId}' and batch '{batchNumber ?? "FEFO"}'.");

            stock = new Stock
            {
                ProductId = productId,
                BranchId = branchId,
                BatchNumber = batchNumber,
                ExpiryDate = expiryDate,
                Quantity = quantityChange,
                ReservedQuantity = 0
            };
            await AddAsync(stock, cancellationToken);
        }
        else
        {
            var newQuantity = (stock.Quantity ?? 0) + quantityChange;
            if (newQuantity < 0)
                throw new InvalidOperationException(
                    $"Insufficient stock for product '{productId}' at branch '{branchId}' and batch '{batchNumber ?? "unspecified"}'.");

            stock.Quantity = newQuantity;
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
            return stock != null
                && (!stock.ExpiryDate.HasValue || stock.ExpiryDate.Value >= DateTime.UtcNow.Date)
                && stock.AvailableQuantity >= quantity;
        }

        // Check total across all non-expired batches for this product+branch.
        var today = DateTime.UtcNow.Date;
        var totalAvailable = await _dbSet
            .Where(s => s.ProductId == productId
                && s.BranchId == branchId
                && !s.IsDeleted
                && (!s.ExpiryDate.HasValue || s.ExpiryDate.Value >= today))
            .SumAsync(s => (s.Quantity ?? 0) - (s.ReservedQuantity ?? 0), cancellationToken);

        return totalAvailable >= quantity;
    }

    public async Task<Stock> ReceiveAsync(
        Guid productId,
        Guid branchId,
        decimal quantity,
        decimal unitCost,
        string batchNumber,
        DateTime expiryDate,
        CancellationToken cancellationToken = default)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));
        if (unitCost < 0)
            throw new ArgumentOutOfRangeException(nameof(unitCost));
        if (string.IsNullOrWhiteSpace(batchNumber))
            throw new ArgumentException("Batch number is required when receiving stock.", nameof(batchNumber));

        var normalizedBatch = batchNumber.Trim();
        var stock = await _dbSet.FirstOrDefaultAsync(s =>
            s.ProductId == productId && s.BranchId == branchId &&
            s.BatchNumber == normalizedBatch && !s.IsDeleted, cancellationToken);
        if (stock == null)
        {
            stock = new Stock
            {
                ProductId = productId,
                BranchId = branchId,
                BatchNumber = batchNumber.Trim(),
                ExpiryDate = expiryDate,
                Quantity = quantity,
                ReservedQuantity = 0,
                AverageCost = unitCost
            };
            await AddAsync(stock, cancellationToken);
            return stock;
        }

        var oldQuantity = stock.Quantity ?? 0;
        var oldValue = oldQuantity * (stock.AverageCost ?? 0);
        stock.Quantity = oldQuantity + quantity;
        stock.AverageCost = stock.Quantity == 0
            ? unitCost
            : Math.Round((oldValue + quantity * unitCost) / stock.Quantity.Value, 4);
        stock.ExpiryDate = expiryDate;
        await UpdateAsync(stock, cancellationToken);
        return stock;
    }

    public async Task<IReadOnlyList<StockAllocation>> DeductAsync(
        Guid productId,
        Guid branchId,
        decimal quantity,
        string? batchNumber = null,
        bool allowExpired = false,
        CancellationToken cancellationToken = default)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Deduction quantity must be greater than zero.");

        var today = DateTime.UtcNow.Date;
        var query = _dbSet.Where(s =>
            s.ProductId == productId &&
            s.BranchId == branchId &&
            !s.IsDeleted &&
            (batchNumber == null || s.BatchNumber == batchNumber) &&
            (allowExpired || !s.ExpiryDate.HasValue || s.ExpiryDate.Value >= today));

        var stocks = await query
            .OrderBy(s => s.ExpiryDate == null)
            .ThenBy(s => s.ExpiryDate)
            .ThenBy(s => s.CreatedAt)
            .ToListAsync(cancellationToken);

        var totalAvailable = stocks.Sum(s => Math.Max(0, s.AvailableQuantity));
        if (totalAvailable < quantity)
            throw new InvalidOperationException(
                $"Insufficient stock for product '{productId}' at branch '{branchId}'. " +
                $"Requested {quantity}, available {totalAvailable}.");

        var remaining = quantity;
        var allocations = new List<StockAllocation>();
        foreach (var stock in stocks)
        {
            if (remaining <= 0)
                break;

            var allocated = Math.Min(Math.Max(0, stock.AvailableQuantity), remaining);
            if (allocated <= 0)
                continue;

            stock.Quantity = (stock.Quantity ?? 0) - allocated;
            allocations.Add(new StockAllocation(
                stock.Oid, stock.BatchNumber, stock.ExpiryDate, allocated, stock.AverageCost));
            remaining -= allocated;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return allocations;
    }
}
