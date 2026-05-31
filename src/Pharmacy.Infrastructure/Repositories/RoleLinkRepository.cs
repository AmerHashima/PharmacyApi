using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class RoleLinkRepository : BaseRepository<RoleLink>, IRoleLinkRepository
{
    public RoleLinkRepository(PharmacyDbContext context) : base(context) { }

    public async Task<IEnumerable<RoleLink>> GetByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(rl => rl.Role)
            .Include(rl => rl.Link)
            .Where(rl => rl.RoleId == roleId && !rl.IsDeleted)
            .OrderBy(rl => rl.Link!.NameEn)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<RoleLink>> GetByLinkIdAsync(Guid linkId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(rl => rl.Role)
            .Include(rl => rl.Link)
            .Where(rl => rl.LinkId == linkId && !rl.IsDeleted)
            .OrderBy(rl => rl.Role!.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<RoleLink?> GetByRoleAndLinkAsync(Guid roleId, Guid linkId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(rl => rl.Role)
            .Include(rl => rl.Link)
            .Where(rl => rl.RoleId == roleId && rl.LinkId == linkId && !rl.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<RoleLink>> GetAccessibleLinksForRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(rl => rl.Role)
            .Include(rl => rl.Link)
            .Where(rl => rl.RoleId == roleId && rl.CanRead && !rl.IsDeleted)
            .OrderBy(rl => rl.Link!.NameEn)
            .ToListAsync(cancellationToken);
    }
}
