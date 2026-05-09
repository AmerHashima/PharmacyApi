using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
    public AccountRepository(PharmacyDbContext context) : base(context) { }

    public async Task<Account?> GetByCodeAsync(string accountCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.AccountCode == accountCode && !a.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Account>> GetByParentAsync(Guid parentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.ParentId == parentId && !a.IsDeleted)
            .OrderBy(a => a.AccountCode)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Account>> GetLeafAccountsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.IsLeaf && a.IsActive && !a.IsDeleted)
            .OrderBy(a => a.AccountCode)
            .ToListAsync(cancellationToken);
    }
}
