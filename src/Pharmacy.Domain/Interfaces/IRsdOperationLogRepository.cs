using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IRsdOperationLogRepository : IBaseRepository<RsdOperationLog>
{
    /// <summary>
    /// Get log with its detail lines
    /// </summary>
    Task<RsdOperationLog?> GetWithDetailsAsync(Guid logId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get logs by branch
    /// </summary>
    Task<IEnumerable<RsdOperationLog>> GetByBranchIdAsync(Guid branchId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get logs by operation type (AppLookupDetail ID)
    /// </summary>
    Task<IEnumerable<RsdOperationLog>> GetByOperationTypeAsync(Guid operationTypeId, CancellationToken cancellationToken = default);
}
