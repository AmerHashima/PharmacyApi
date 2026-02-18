using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

/// <summary>
/// Repository interface for SalesInvoice entity operations.
/// </summary>
public interface ISalesInvoiceRepository : IBaseRepository<SalesInvoice>
{
    /// <summary>
    /// Get invoice by number
    /// </summary>
    Task<SalesInvoice?> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get invoices by branch
    /// </summary>
    Task<IEnumerable<SalesInvoice>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get invoices by cashier
    /// </summary>
    Task<IEnumerable<SalesInvoice>> GetByCashierAsync(Guid cashierId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get invoices within a date range
    /// </summary>
    Task<IEnumerable<SalesInvoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? branchId = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get invoice with items
    /// </summary>
    Task<SalesInvoice?> GetWithItemsAsync(Guid invoiceId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Generate a unique invoice number
    /// </summary>
    Task<string> GenerateInvoiceNumberAsync(Guid branchId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get daily sales total for a branch
    /// </summary>
    Task<decimal> GetDailySalesTotalAsync(Guid branchId, DateTime date, CancellationToken cancellationToken = default);
}
