using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class CashBoxRepository : BaseRepository<CashBox>, ICashBoxRepository
{
    public CashBoxRepository(PharmacyDbContext context) : base(context) { }

    public async Task<IEnumerable<CashBox>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(cb => cb.BranchId == branchId && !cb.IsDeleted)
            .OrderBy(cb => cb.Code)
            .ToListAsync(cancellationToken);
    }
}
