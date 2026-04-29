using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class ReportParameterRepository : BaseRepository<ReportParameter>, IReportParameterRepository
{
    public ReportParameterRepository(PharmacyDbContext context) : base(context) { }

    public async Task<IEnumerable<ReportParameter>> GetByLinkIdAsync(Guid linkId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.LinksOid == linkId && !p.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteByLinkIdAsync(Guid linkId, CancellationToken cancellationToken = default)
    {
        var parameters = await _dbSet
            .Where(p => p.LinksOid == linkId)
            .ToListAsync(cancellationToken);

        foreach (var param in parameters)
        {
            param.IsDeleted = true;
            param.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
