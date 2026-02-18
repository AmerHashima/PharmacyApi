using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

/// <summary>
/// Repository interface for Stakeholder entity operations.
/// Handles pharmacies, suppliers, and other external entities.
/// </summary>
public interface IStakeholderRepository : IBaseRepository<Stakeholder>
{
    /// <summary>
    /// Get stakeholders by type
    /// </summary>
    Task<IEnumerable<Stakeholder>> GetByTypeAsync(Guid stakeholderTypeId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get stakeholder with its branches
    /// </summary>
    Task<Stakeholder?> GetWithBranchesAsync(Guid stakeholderId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get active suppliers
    /// </summary>
    Task<IEnumerable<Stakeholder>> GetActiveSuppliersAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Check if GLN is unique
    /// </summary>
    Task<bool> IsGLNUniqueAsync(string gln, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
