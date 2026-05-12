using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class PaymentVoucherDetailRepository : BaseRepository<PaymentVoucherDetail>, IPaymentVoucherDetailRepository
{
    public PaymentVoucherDetailRepository(PharmacyDbContext context) : base(context) { }

    public async Task<IEnumerable<PaymentVoucherDetail>> GetByVoucherIdAsync(Guid paymentVoucherId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Account)
            .Include(d => d.CostCenter)
            .Where(d => d.PaymentVoucherId == paymentVoucherId && !d.IsDeleted)
            .OrderBy(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
