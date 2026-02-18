using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

/// <summary>
/// Repository interface for Branch entity operations.
/// </summary>
public interface IBranchRepository : IBaseRepository<Branch>
{
    /// <summary>
    /// Check if branch code is unique
    /// </summary>
    Task<bool> IsBranchCodeUniqueAsync(string branchCode, Guid? excludeId = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get branch by code
    /// </summary>
    Task<Branch?> GetByCodeAsync(string branchCode, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get branch with its users
    /// </summary>
    Task<Branch?> GetWithUsersAsync(Guid branchId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get branch with its stock
    /// </summary>
    Task<Branch?> GetWithStockAsync(Guid branchId, CancellationToken cancellationToken = default);
}
