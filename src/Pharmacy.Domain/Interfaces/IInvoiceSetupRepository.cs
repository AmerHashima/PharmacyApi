using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IInvoiceSetupRepository : IBaseRepository<InvoiceSetup>
{
    /// <summary>Get all setups for a specific branch.</summary>
    Task<IEnumerable<InvoiceSetup>> GetByBranchIdAsync(Guid branchId, CancellationToken cancellationToken = default);

    /// <summary>Get all global template rows (BranchId is null).</summary>
    Task<IEnumerable<InvoiceSetup>> GetGlobalTemplatesAsync(CancellationToken cancellationToken = default);

    /// <summary>Find a specific invoice type for a branch by Format code.</summary>
    Task<InvoiceSetup?> GetByBranchAndFormatAsync(Guid branchId, string format, CancellationToken cancellationToken = default);
}
