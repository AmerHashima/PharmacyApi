using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IAppLookupDetailRepository : IBaseRepository<AppLookupDetail>
{
    Task<IEnumerable<AppLookupDetail>> GetByMasterIdAsync(Guid masterID, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppLookupDetail>> GetByLookupCodeAsync(string lookupCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppLookupDetail>> GetByMasterCodeAsync(string masterCode, CancellationToken cancellationToken = default);
    Task<AppLookupDetail?> GetByValueCodeAsync(Guid masterID, string valueCode, CancellationToken cancellationToken = default);
    Task<AppLookupDetail?> GetDefaultValueAsync(Guid masterID, CancellationToken cancellationToken = default);
    Task<bool> ValueCodeExistsAsync(Guid masterID, string valueCode, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppLookupDetail>> GetOrderedByMasterIdAsync(Guid masterID, CancellationToken cancellationToken = default);
}