using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

/// <summary>
/// Repository interface for ReturnInvoice entity operations.
/// </summary>
public interface IReturnInvoiceRepository : IBaseRepository<ReturnInvoice>
{
    /// <summary>
    /// Get return invoice by return number
    /// </summary>
    Task<ReturnInvoice?> GetByReturnNumberAsync(string returnNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get return invoices by original sales invoice
    /// </summary>
    Task<IEnumerable<ReturnInvoice>> GetByOriginalInvoiceAsync(Guid originalInvoiceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get return invoices by branch
    /// </summary>
    Task<IEnumerable<ReturnInvoice>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get return invoices by cashier
    /// </summary>
    Task<IEnumerable<ReturnInvoice>> GetByCashierAsync(Guid cashierId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get return invoices within a date range
    /// </summary>
    Task<IEnumerable<ReturnInvoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? branchId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get return invoice with items included
    /// </summary>
    Task<ReturnInvoice?> GetWithItemsAsync(Guid returnInvoiceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate a unique return number
    /// </summary>
    Task<string> GenerateReturnNumberAsync(Guid branchId, CancellationToken cancellationToken = default);
}
