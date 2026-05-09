using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;

namespace Pharmacy.Domain.Interfaces.Accounting;

public interface IJournalEntryDetailRepository : IBaseRepository<JournalEntryDetail>
{
    Task<IEnumerable<JournalEntryDetail>> GetByJournalEntryAsync(Guid journalEntryId, CancellationToken cancellationToken = default);
}
