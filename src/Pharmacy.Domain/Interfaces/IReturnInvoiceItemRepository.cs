using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

/// <summary>
/// Repository interface for ReturnInvoiceItem entity operations.
/// </summary>
public interface IReturnInvoiceItemRepository : IBaseRepository<ReturnInvoiceItem>
{
    /// <summary>
    /// Get items by return invoice
    /// </summary>
    Task<IEnumerable<ReturnInvoiceItem>> GetByReturnInvoiceAsync(Guid returnInvoiceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get items by product
    /// </summary>
    Task<IEnumerable<ReturnInvoiceItem>> GetByProductAsync(Guid productId, CancellationToken cancellationToken = default);
}
