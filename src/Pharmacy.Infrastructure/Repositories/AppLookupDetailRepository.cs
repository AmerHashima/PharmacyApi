using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.Infrastructure.Repositories;

public class AppLookupDetailRepository : BaseRepository<AppLookupDetail>, IAppLookupDetailRepository
{
    public AppLookupDetailRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<AppLookupDetail>> GetByMasterIdAsync(Guid masterID, CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupDetails
            .Include(x => x.Master)
            .Where(x => !x.IsDeleted && x.MasterID == masterID)
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AppLookupDetail>> GetByLookupCodeAsync(string lookupCode, CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupDetails
            .Include(x => x.Master)
            .Where(x => !x.IsDeleted && x.Master.LookupCode == lookupCode)
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AppLookupDetail>> GetByMasterCodeAsync(string masterCode, CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupDetails
            .Include(x => x.Master)
            .Where(x => !x.IsDeleted && x.Master.LookupCode == masterCode && x.IsActive)
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<AppLookupDetail?> GetByValueCodeAsync(Guid masterID, string valueCode, CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupDetails
            .Include(x => x.Master)
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.MasterID == masterID && x.ValueCode == valueCode, cancellationToken);
    }

    public async Task<AppLookupDetail?> GetDefaultValueAsync(Guid masterID, CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupDetails
            .Include(x => x.Master)
            .Where(x => !x.IsDeleted && x.MasterID == masterID)
            .FirstOrDefaultAsync(x => x.IsDefault, cancellationToken);
    }

    public async Task<bool> ValueCodeExistsAsync(Guid masterID, string valueCode, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.AppLookupDetails
            .Where(x => !x.IsDeleted && x.MasterID == masterID && x.ValueCode == valueCode);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Oid != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<IEnumerable<AppLookupDetail>> GetOrderedByMasterIdAsync(Guid masterID, CancellationToken cancellationToken = default)
    {
        return await _context.AppLookupDetails
            .Include(x => x.Master)
            .Where(x => !x.IsDeleted && x.MasterID == masterID)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.ValueNameEn)
            .ToListAsync(cancellationToken);
    }
}