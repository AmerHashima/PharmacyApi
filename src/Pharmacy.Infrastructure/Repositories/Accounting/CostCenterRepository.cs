using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class CostCenterRepository : BaseRepository<CostCenter>, ICostCenterRepository
{
    public CostCenterRepository(PharmacyDbContext context) : base(context) { }

    public async Task<IEnumerable<CostCenter>> GetByParentAsync(Guid parentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.ParentId == parentId && !c.IsDeleted)
            .OrderBy(c => c.Code)
            .ToListAsync(cancellationToken);
    }
}
