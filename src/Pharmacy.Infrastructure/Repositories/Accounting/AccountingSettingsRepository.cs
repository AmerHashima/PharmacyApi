using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class AccountingSettingsRepository : BaseRepository<AccountingSettings>, IAccountingSettingsRepository
{
    public AccountingSettingsRepository(PharmacyDbContext context) : base(context) { }

    public async Task<AccountingSettings?> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Branch)
            .Include(s => s.SalesAccount)
            .Include(s => s.VatAccount)
            .Include(s => s.DiscountAccount)
            .Include(s => s.CogsAccount)
            .Include(s => s.InventoryAccount)
            .Include(s => s.CashAccount)
            .Include(s => s.BankAccount)
            .Include(s => s.ReceivableAccount)
            .Where(s => s.BranchId == branchId && !s.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
