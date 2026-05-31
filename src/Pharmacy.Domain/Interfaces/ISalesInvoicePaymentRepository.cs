using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface ISalesInvoicePaymentRepository : IBaseRepository<SalesInvoicePayment>
{
    Task<IEnumerable<SalesInvoicePayment>> GetBySalesInvoiceAsync(Guid salesInvoiceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SalesInvoicePayment>> GetByShiftAsync(Guid shiftId, CancellationToken cancellationToken = default);
}
