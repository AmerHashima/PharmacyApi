using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

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
}
