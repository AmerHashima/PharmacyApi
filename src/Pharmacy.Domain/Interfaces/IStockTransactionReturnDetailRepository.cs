using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

/// <summary>
/// Repository interface for StockTransactionReturnDetail entity operations
/// </summary>
public interface IStockTransactionReturnDetailRepository : IBaseRepository<StockTransactionReturnDetail>
{
    /// <summary>
    /// Get all details for a specific return transaction
    /// </summary>
    Task<IEnumerable<StockTransactionReturnDetail>> GetByTransactionReturnIdAsync(Guid transactionReturnId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get details by product
    /// </summary>
    Task<IEnumerable<StockTransactionReturnDetail>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get details by batch number
    /// </summary>
    Task<IEnumerable<StockTransactionReturnDetail>> GetByBatchNumberAsync(string batchNumber, CancellationToken cancellationToken = default);
}
