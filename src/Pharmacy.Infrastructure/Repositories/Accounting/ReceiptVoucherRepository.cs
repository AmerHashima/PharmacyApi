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
            .Include(rv => rv.FiscalYear)
            .Include(rv => rv.Branch)
            .Include(rv => rv.CashBox)
            .Include(rv => rv.BankAccount)
            .Include(rv => rv.JournalEntry)
            .Include(rv => rv.Details)
                .ThenInclude(d => d.Account)
            .Include(rv => rv.Details)
                .ThenInclude(d => d.CostCenter)
            .Include(rv => rv.Details)
                .ThenInclude(d => d.Customer)
            .Where(rv => rv.Oid == id && !rv.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
