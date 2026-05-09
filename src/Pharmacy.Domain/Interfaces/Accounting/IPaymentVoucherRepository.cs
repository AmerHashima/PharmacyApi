using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Domain.Interfaces.Accounting;

public interface IPaymentVoucherRepository : IBaseRepository<PaymentVoucher>
{
    Task<PaymentVoucher?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
}
