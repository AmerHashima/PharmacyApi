using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IRoleLinkRepository : IBaseRepository<RoleLink>
{
    /// <summary>Get all RoleLinks for a given role, including the Link navigation.</summary>
    Task<IEnumerable<RoleLink>> GetByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);

    /// <summary>Get all RoleLinks for a given link.</summary>
    Task<IEnumerable<RoleLink>> GetByLinkIdAsync(Guid linkId, CancellationToken cancellationToken = default);

    /// <summary>Get a single RoleLink by role + link combination.</summary>
    Task<RoleLink?> GetByRoleAndLinkAsync(Guid roleId, Guid linkId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all links accessible (CanRead=true) for a given role,
    /// including the Link's full details.
    /// </summary>
    Task<IEnumerable<RoleLink>> GetAccessibleLinksForRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
}
