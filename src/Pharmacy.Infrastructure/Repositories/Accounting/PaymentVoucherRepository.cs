using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class PaymentVoucherRepository : BaseRepository<PaymentVoucher>, IPaymentVoucherRepository
{
    public PaymentVoucherRepository(PharmacyDbContext context) : base(context) { }

    public async Task<PaymentVoucher?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(pv => pv.Stakeholder)
            .Include(pv => pv.CashBox)
            .Include(pv => pv.BankAccount)
            .Include(pv => pv.JournalEntry)
            .Where(pv => pv.Oid == id && !pv.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
