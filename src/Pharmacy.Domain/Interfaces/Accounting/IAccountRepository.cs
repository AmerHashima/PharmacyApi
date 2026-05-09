using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Domain.Interfaces.Accounting;

public interface IAccountRepository : IBaseRepository<Account>
{
    Task<Account?> GetByCodeAsync(string accountCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<Account>> GetByParentAsync(Guid parentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Account>> GetLeafAccountsAsync(CancellationToken cancellationToken = default);
}
