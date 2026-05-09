using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Domain.Interfaces.Accounting;

public interface IReceiptVoucherRepository : IBaseRepository<ReceiptVoucher>
{
    Task<ReceiptVoucher?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
}
