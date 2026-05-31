using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IPurchaseInvoiceRepository : IBaseRepository<PurchaseInvoice>
{
    Task<PurchaseInvoice?> GetWithPaymentsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseInvoice>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseInvoice>> GetBySupplierAsync(Guid supplierId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseInvoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? branchId = null, CancellationToken cancellationToken = default);
    Task<PurchaseInvoice?> GetByInvoiceNumberAsync(string number, CancellationToken cancellationToken = default);
}
