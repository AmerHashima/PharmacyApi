using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IPurchaseInvoicePaymentRepository : IBaseRepository<PurchaseInvoicePayment>
{
    Task<IEnumerable<PurchaseInvoicePayment>> GetByPurchaseInvoiceAsync(Guid purchaseInvoiceId, CancellationToken cancellationToken = default);
}
