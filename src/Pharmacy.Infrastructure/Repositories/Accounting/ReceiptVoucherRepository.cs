using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class ReceiptVoucherRepository : BaseRepository<ReceiptVoucher>, IReceiptVoucherRepository
{
    public ReceiptVoucherRepository(PharmacyDbContext context) : base(context) { }

    public async Task<ReceiptVoucher?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(rv => rv.Customer)
            .Include(rv => rv.CashBox)
            .Include(rv => rv.BankAccount)
            .Include(rv => rv.JournalEntry)
            .Where(rv => rv.Oid == id && !rv.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
