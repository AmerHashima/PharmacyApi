using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Domain.Interfaces.Accounting;

public interface IPaymentVoucherDetailRepository : IBaseRepository<PaymentVoucherDetail>
{
    Task<IEnumerable<PaymentVoucherDetail>> GetByVoucherIdAsync(Guid paymentVoucherId, CancellationToken cancellationToken = default);
}
