using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Domain.Interfaces.Accounting;

public interface ICostCenterRepository : IBaseRepository<CostCenter>
{
    Task<IEnumerable<CostCenter>> GetByParentAsync(Guid parentId, CancellationToken cancellationToken = default);
}
