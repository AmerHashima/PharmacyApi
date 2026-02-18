using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

/// <summary>
/// Repository interface for ProductBatch entity operations.
/// </summary>
public interface IProductBatchRepository : IBaseRepository<ProductBatch>
{
    /// <summary>
    /// Get batches by product
    /// </summary>
    Task<IEnumerable<ProductBatch>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get batches by branch
    /// </summary>
    Task<IEnumerable<ProductBatch>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get batch by batch number and product
    /// </summary>
    Task<ProductBatch?> GetByBatchNumberAsync(Guid productId, string batchNumber, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get expiring batches within days
    /// </summary>
    Task<IEnumerable<ProductBatch>> GetExpiringBatchesAsync(Guid branchId, int daysUntilExpiry, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get expired batches
    /// </summary>
    Task<IEnumerable<ProductBatch>> GetExpiredBatchesAsync(Guid branchId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get oldest batch with available quantity (FEFO - First Expiry First Out)
    /// </summary>
    Task<ProductBatch?> GetOldestAvailableBatchAsync(Guid productId, Guid branchId, CancellationToken cancellationToken = default);
}
