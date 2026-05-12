using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class ReceiptVoucherDetailRepository : BaseRepository<ReceiptVoucherDetail>, IReceiptVoucherDetailRepository
{
    public ReceiptVoucherDetailRepository(PharmacyDbContext context) : base(context) { }

    public async Task<IEnumerable<ReceiptVoucherDetail>> GetByVoucherIdAsync(Guid receiptVoucherId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Account)
            .Include(d => d.CostCenter)
            .Where(d => d.ReceiptVoucherId == receiptVoucherId && !d.IsDeleted)
            .OrderBy(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
