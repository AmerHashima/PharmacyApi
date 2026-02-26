using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IBranchIntegrationSettingRepository : IBaseRepository<BranchIntegrationSetting>
{
    Task<IEnumerable<BranchIntegrationSetting>> GetByBranchIdAsync(Guid branchId);
    Task<IEnumerable<BranchIntegrationSetting>> GetByProviderIdAsync(Guid providerId);
    Task<BranchIntegrationSetting?> GetByBranchAndProviderAsync(Guid branchId, Guid providerId);
    Task<bool> ExistsAsync(Guid branchId, Guid providerId, Guid? excludeId = null);
}