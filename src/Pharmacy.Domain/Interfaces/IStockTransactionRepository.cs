using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

/// <summary>
/// Repository interface for StockTransaction entity operations.
/// </summary>
public interface IStockTransactionRepository : IBaseRepository<StockTransaction>
{
    /// <summary>
    /// Get transactions by type
    /// </summary>
    Task<IEnumerable<StockTransaction>> GetByTypeAsync(Guid transactionTypeId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get transactions for a product
    /// </summary>
    Task<IEnumerable<StockTransaction>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get transactions for a branch (incoming or outgoing)
    /// </summary>
    Task<IEnumerable<StockTransaction>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get transactions within a date range
    /// </summary>
    Task<IEnumerable<StockTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? branchId = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get transaction by reference number
    /// </summary>
    Task<StockTransaction?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Generate a unique reference number
    /// </summary>
    Task<string> GenerateReferenceNumberAsync(string prefix, CancellationToken cancellationToken = default);
}
