using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Domain.Interfaces.Accounting;

public interface IAccountingSettingsRepository : IBaseRepository<AccountingSettings>
{
    Task<AccountingSettings?> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default);
}
