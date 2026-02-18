using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> RoleNameExistsAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default);
}