using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class CashierShiftDetailRepository : BaseRepository<CashierShiftDetail>, ICashierShiftDetailRepository
{
    public CashierShiftDetailRepository(PharmacyDbContext context) : base(context) { }

    public async Task<IEnumerable<CashierShiftDetail>> GetByShiftAsync(Guid shiftId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.TransactionType)
            .Include(x => x.PaymentMethod)
            .Where(x => x.ShiftId == shiftId && !x.IsDeleted)
            .OrderBy(x => x.TransactionDate)
            .ToListAsync(cancellationToken);
    }
}
