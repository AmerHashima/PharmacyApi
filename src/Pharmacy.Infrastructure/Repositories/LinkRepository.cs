using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class LinkRepository : BaseRepository<Link>, ILinkRepository
{
    public LinkRepository(PharmacyDbContext context) : base(context) { }

    public async Task<Link?> GetWithParametersAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.ReportParameters.Where(p => !p.IsDeleted))
            .Where(l => l.Oid == id && !l.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Link>> GetActiveLinksAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.ReportParameters.Where(p => !p.IsDeleted))
            .Where(l => !l.IsDeleted && l.Active == true)
            .OrderBy(l => l.NameEn)
            .ToListAsync(cancellationToken);
    }
}
