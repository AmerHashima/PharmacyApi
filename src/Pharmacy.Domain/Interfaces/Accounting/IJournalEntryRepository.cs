using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Domain.Interfaces.Accounting;

public interface IJournalEntryRepository : IBaseRepository<JournalEntry>
{
    Task<JournalEntry?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<JournalEntry>> GetByFiscalYearAsync(Guid fiscalYearId, CancellationToken cancellationToken = default);
}
