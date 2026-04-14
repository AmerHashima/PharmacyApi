using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IInvoiceShapeRepository : IBaseRepository<InvoiceShape>
{
    Task<IEnumerable<InvoiceShape>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default);
    Task<InvoiceShape?> GetByNameAsync(string shapeName, CancellationToken cancellationToken = default);
    Task<bool> NameExistsAsync(string shapeName, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
