using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public sealed record StockAllocation(
    Guid StockId,
    string? BatchNumber,
    DateTime? ExpiryDate,
    decimal Quantity,
    decimal? AverageCost);

/// <summary>
/// Repository interface for Stock entity operations.
/// </summary>
public interface IStockRepository : IBaseRepository<Stock>
{
    /// <summary>
    /// Get stock by product, branch, and batch number
    /// </summary>
    Task<Stock?> GetByProductAndBranchAsync(Guid productId, Guid branchId, string? batchNumber = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all stock for a branch
    /// </summary>
    Task<IEnumerable<Stock>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all stock for a product across branches
    /// </summary>
    Task<IEnumerable<Stock>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update stock quantity for a specific product+branch+batch
    /// </summary>
    Task<Stock> UpdateQuantityAsync(Guid productId, Guid branchId, decimal quantityChange, string? batchNumber = null, DateTime? expiryDate = null, CancellationToken cancellationToken = default);

    /// <summary>Receives stock and recalculates its weighted-average unit cost.</summary>
    Task<Stock> ReceiveAsync(
        Guid productId,
        Guid branchId,
        decimal quantity,
        decimal unitCost,
        string batchNumber,
        DateTime expiryDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Reserve stock for pending transactions
    /// </summary>
    Task<bool> ReserveStockAsync(Guid productId, Guid branchId, decimal quantity, string? batchNumber = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Release reserved stock
    /// </summary>
    Task<bool> ReleaseReservedStockAsync(Guid productId, Guid branchId, decimal quantity, string? batchNumber = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if sufficient stock is available (across all batches if batchNumber is null)
    /// </summary>
    Task<bool> HasSufficientStockAsync(Guid productId, Guid branchId, decimal quantity, string? batchNumber = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deducts stock atomically. When no batch is supplied, batches are consumed by
    /// FEFO (earliest expiry first). Must be called inside the business transaction.
    /// </summary>
    Task<IReadOnlyList<StockAllocation>> DeductAsync(
        Guid productId,
        Guid branchId,
        decimal quantity,
        string? batchNumber = null,
        bool allowExpired = false,
        CancellationToken cancellationToken = default);
}
