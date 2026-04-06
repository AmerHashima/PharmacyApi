using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

/// <summary>
/// Repository interface for Store entity operations.
/// </summary>
public interface IStoreRepository : IBaseRepository<Store>
{
    /// <summary>
    /// Get all stores for a specific branch
    /// </summary>
    Task<IEnumerable<Store>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a store by its unique code
    /// </summary>
    Task<Store?> GetByCodeAsync(string storeCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a store code already exists (for uniqueness validation)
    /// </summary>
    Task<bool> CodeExistsAsync(string storeCode, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
