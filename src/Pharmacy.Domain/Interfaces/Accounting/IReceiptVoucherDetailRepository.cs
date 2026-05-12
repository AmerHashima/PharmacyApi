using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Domain.Interfaces.Accounting;

public interface IReceiptVoucherDetailRepository : IBaseRepository<ReceiptVoucherDetail>
{
    Task<IEnumerable<ReceiptVoucherDetail>> GetByVoucherIdAsync(Guid receiptVoucherId, CancellationToken cancellationToken = default);
}
