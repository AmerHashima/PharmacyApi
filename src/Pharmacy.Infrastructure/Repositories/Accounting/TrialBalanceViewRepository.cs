using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class TrialBalanceViewRepository(PharmacyDbContext context) : ITrialBalanceViewRepository
{
    public IQueryable<TrialBalanceViewRow> GetQueryable() =>
        context.TrialBalanceViewRows;
}
