using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface ICashierShiftDetailRepository : IBaseRepository<CashierShiftDetail>
{
    Task<IEnumerable<CashierShiftDetail>> GetByShiftAsync(Guid shiftId, CancellationToken cancellationToken = default);
}
