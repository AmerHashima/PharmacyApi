using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class FiscalYearRepository : BaseRepository<FiscalYear>, IFiscalYearRepository
{
    public FiscalYearRepository(PharmacyDbContext context) : base(context) { }

    public async Task<FiscalYear?> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet
            .Where(f => !f.IsDeleted && !f.IsClosed && f.StartDate <= today && f.EndDate >= today)
            .OrderByDescending(f => f.StartDate)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
