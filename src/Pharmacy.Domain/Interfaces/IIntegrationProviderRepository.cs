using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IIntegrationProviderRepository : IBaseRepository<IntegrationProvider>
{
    Task<IntegrationProvider?> GetByNameAsync(string name);
    Task<IEnumerable<IntegrationProvider>> GetActiveProvidersAsync();
    Task<bool> ExistsAsync(string name, Guid? excludeId = null);
}