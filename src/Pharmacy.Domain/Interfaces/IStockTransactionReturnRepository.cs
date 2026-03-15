using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

/// <summary>
/// Repository interface for StockTransactionReturn entity operations.
/// </summary>
public interface IStockTransactionReturnRepository : IBaseRepository<StockTransactionReturn>
{
    /// <summary>
    /// Get return transactions by type
    /// </summary>
    Task<IEnumerable<StockTransactionReturn>> GetByTypeAsync(Guid transactionTypeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get return transactions for a branch (incoming or outgoing)
    /// </summary>
    Task<IEnumerable<StockTransactionReturn>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get return transactions within a date range
    /// </summary>
    Task<IEnumerable<StockTransactionReturn>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? branchId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get return transactions linked to a specific return invoice
    /// </summary>
    Task<IEnumerable<StockTransactionReturn>> GetByReturnInvoiceIdAsync(Guid returnInvoiceId, CancellationToken cancellationToken = default);
}
