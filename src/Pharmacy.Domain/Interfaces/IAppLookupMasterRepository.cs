using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IAppLookupMasterRepository : IBaseRepository<AppLookupMaster> // Changed from IRepository<AppLookupMaster>
{
    Task<AppLookupMaster?> GetByLookupCodeAsync(string lookupCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppLookupMaster>> GetSystemLookupsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AppLookupMaster>> GetCustomLookupsAsync(CancellationToken cancellationToken = default);
    Task<bool> LookupCodeExistsAsync(string lookupCode, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task<AppLookupMaster?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AppLookupMaster?> GetByCodeWithDetailsAsync(string lookupCode, CancellationToken cancellationToken = default);
}