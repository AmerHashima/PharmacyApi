using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.Infrastructure.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<bool> RoleNameExistsAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Roles.Where(x => !x.IsDeleted && x.Name == name);
        
        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Oid != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}