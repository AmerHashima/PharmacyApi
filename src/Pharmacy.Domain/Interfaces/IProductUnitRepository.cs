using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IProductUnitRepository : IBaseRepository<ProductUnit>
{
    Task<IEnumerable<ProductUnit>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<ProductUnit?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductUnit?> GetByProductAndPackageTypeAsync(Guid productId, Guid packageTypeId, CancellationToken cancellationToken = default);
}
