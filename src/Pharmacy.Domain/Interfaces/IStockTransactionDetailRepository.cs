using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

/// <summary>
/// Repository interface for StockTransactionDetail entity operations
/// </summary>
public interface IStockTransactionDetailRepository : IBaseRepository<StockTransactionDetail>
{
    /// <summary>
    /// Get all details for a specific transaction
    /// </summary>
    Task<IEnumerable<StockTransactionDetail>> GetByTransactionIdAsync(Guid transactionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get details by product
    /// </summary>
    Task<IEnumerable<StockTransactionDetail>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get details by batch number
    /// </summary>
    Task<IEnumerable<StockTransactionDetail>> GetByBatchNumberAsync(string batchNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get details by GTIN
    /// </summary>
    Task<IEnumerable<StockTransactionDetail>> GetByGTINAsync(string gtin, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete all details for a transaction
    /// </summary>
    Task DeleteByTransactionIdAsync(Guid transactionId, CancellationToken cancellationToken = default);
}
