using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Domain.Interfaces.Accounting;

public interface IFiscalYearRepository : IBaseRepository<FiscalYear>
{
    Task<FiscalYear?> GetCurrentAsync(CancellationToken cancellationToken = default);
}
