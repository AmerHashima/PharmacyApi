using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Domain.Interfaces.Accounting;

public interface ICashBoxRepository : IBaseRepository<CashBox>
{
    Task<IEnumerable<CashBox>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default);
}
