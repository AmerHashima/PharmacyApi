using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class GenericNameRepository : BaseRepository<GenericName>, IGenericNameRepository
{
    public GenericNameRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<GenericName>> SearchAsync(string term, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(g => !g.IsDeleted &&
                        (g.NameEN.Contains(term) || g.NameAR.Contains(term)))
            .OrderBy(g => g.NameEN)
            .ToListAsync(cancellationToken);
    }
}
