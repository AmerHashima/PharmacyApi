using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface ICashierShiftRepository : IBaseRepository<CashierShift>
{
    Task<CashierShift?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CashierShift>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CashierShift>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<CashierShift?> GetOpenShiftAsync(Guid branchId, Guid userId, CancellationToken cancellationToken = default);
}
