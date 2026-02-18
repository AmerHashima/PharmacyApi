using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Pharmacy.Infrastructure.Repositories;

public class AppLookupMasterRepository : BaseRepository<AppLookupMaster>, IAppLookupMasterRepository
{
    public AppLookupMasterRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<AppLookupMaster?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupMasters
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Oid == id, cancellationToken);
    }

    public async Task<IEnumerable<AppLookupMaster>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupMasters
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.LookupCode)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AppLookupMaster>> FindAsync(Expression<Func<AppLookupMaster, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupMasters
            .Where(x => !x.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<AppLookupMaster> AddAsync(AppLookupMaster entity, CancellationToken cancellationToken = default)
    {
        _context.AppLookupMasters.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(AppLookupMaster entity, CancellationToken cancellationToken = default)
    {
        _context.AppLookupMasters.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            await UpdateAsync(entity, cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupMasters
            .AnyAsync(x => x.Oid == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupMasters
            .CountAsync(x => !x.IsDeleted, cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<AppLookupMaster, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupMasters
            .Where(x => !x.IsDeleted)
            .CountAsync(predicate, cancellationToken);
    }

    public async Task<AppLookupMaster?> GetByLookupCodeAsync(string lookupCode, CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupMasters
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.LookupCode == lookupCode, cancellationToken);
    }

    public async Task<IEnumerable<AppLookupMaster>> GetSystemLookupsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupMasters
            .Where(x => !x.IsDeleted && x.IsSystem)
            .OrderBy(x => x.LookupCode)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AppLookupMaster>> GetCustomLookupsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupMasters
            .Where(x => !x.IsDeleted && !x.IsSystem)
            .OrderBy(x => x.LookupCode)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> LookupCodeExistsAsync(string lookupCode, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.AppLookupMasters.Where(x => !x.IsDeleted && x.LookupCode == lookupCode);
        
        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Oid != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<AppLookupMaster?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupMasters
            .Include(x => x.LookupDetails.Where(d => !d.IsDeleted).OrderBy(d => d.SortOrder))
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Oid == id, cancellationToken);
    }

    public async Task<AppLookupMaster?> GetByCodeWithDetailsAsync(string lookupCode, CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupMasters
            .Include(x => x.LookupDetails.Where(d => !d.IsDeleted).OrderBy(d => d.SortOrder))
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.LookupCode == lookupCode, cancellationToken);
    }
}