using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Domain.Interfaces.Accounting;

public interface IBankAccountRepository : IBaseRepository<BankAccount>
{
    Task<IEnumerable<BankAccount>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default);
}
