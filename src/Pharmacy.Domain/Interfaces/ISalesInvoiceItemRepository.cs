using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

/// <summary>
/// Repository interface for SalesInvoiceItem entity operations.
/// </summary>
public interface ISalesInvoiceItemRepository : IBaseRepository<SalesInvoiceItem>
{
    /// <summary>
    /// Get items by invoice
    /// </summary>
    Task<IEnumerable<SalesInvoiceItem>> GetByInvoiceAsync(Guid invoiceId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get items by product
    /// </summary>
    Task<IEnumerable<SalesInvoiceItem>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get top selling products for a branch
    /// </summary>
    Task<IEnumerable<(Guid ProductId, string ProductName, decimal TotalQuantity, decimal TotalRevenue)>> GetTopSellingProductsAsync(
        Guid branchId, 
        DateTime startDate, 
        DateTime endDate, 
        int top = 10, 
        CancellationToken cancellationToken = default);
}
