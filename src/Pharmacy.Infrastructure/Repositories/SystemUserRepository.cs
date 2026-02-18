using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.Infrastructure.Repositories;

public class SystemUserRepository : BaseRepository<SystemUser>, ISystemUserRepository
{
    public SystemUserRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<SystemUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.SystemUsers
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }

    public async Task<SystemUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.SystemUsers
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<bool> IsUsernameUniqueAsync(string username, Guid? excludeUserId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.SystemUsers.Where(x => !x.IsDeleted && x.Username == username);

        if (excludeUserId.HasValue)
        {
            query = query.Where(x => x.Oid != excludeUserId.Value);
        }

        return !await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeUserId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.SystemUsers.Where(x => !x.IsDeleted && x.Email == email);

        if (excludeUserId.HasValue)
        {
            query = query.Where(x => x.Oid != excludeUserId.Value);
        }

        return !await query.AnyAsync(cancellationToken);
    }

    public async Task<IEnumerable<SystemUser>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SystemUsers
            .Where(x => !x.IsDeleted && x.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SystemUser>> GetUsersByRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await _context.SystemUsers
            .Where(x => !x.IsDeleted && x.RoleId == roleId)
            .ToListAsync(cancellationToken);
    }
}