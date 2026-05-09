using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class BankAccountRepository : BaseRepository<BankAccount>, IBankAccountRepository
{
    public BankAccountRepository(PharmacyDbContext context) : base(context) { }

    public async Task<IEnumerable<BankAccount>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ba => ba.BranchId == branchId && !ba.IsDeleted)
            .OrderBy(ba => ba.Code)
            .ToListAsync(cancellationToken);
    }
}
