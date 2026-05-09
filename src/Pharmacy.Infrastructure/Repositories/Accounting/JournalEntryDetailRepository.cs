using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class JournalEntryDetailRepository : BaseRepository<JournalEntryDetail>, IJournalEntryDetailRepository
{
    public JournalEntryDetailRepository(PharmacyDbContext context) : base(context) { }

    public async Task<IEnumerable<JournalEntryDetail>> GetByJournalEntryAsync(Guid journalEntryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Account)
            .Include(d => d.CostCenter)
            .Where(d => d.JournalEntryId == journalEntryId && !d.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}
