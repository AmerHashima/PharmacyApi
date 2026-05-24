using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Domain.Interfaces.Accounting;

public interface ITrialBalanceViewRepository
{
    IQueryable<TrialBalanceViewRow> GetQueryable();
}
