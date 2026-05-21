using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;

namespace Pharmacy.Infrastructure.Repositories.Accounting;

public class JournalEntryRepository : BaseRepository<JournalEntry>, IJournalEntryRepository
{
    public JournalEntryRepository(PharmacyDbContext context) : base(context) { }

    public async Task<JournalEntry?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(j => j.FiscalYear)
            .Include(j => j.Branch)
            .Include(j => j.Details.OrderBy(d => d.LineNumber))
                .ThenInclude(d => d.Account)
            .Include(j => j.Details.OrderBy(d => d.LineNumber))
                .ThenInclude(d => d.CostCenter)
            .Where(j => j.Oid == id && !j.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<JournalEntry>> GetByFiscalYearAsync(Guid fiscalYearId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(j => j.Branch)
            .Where(j => j.FiscalYearId == fiscalYearId && !j.IsDeleted)
            .OrderByDescending(j => j.EntryDate)
            .ToListAsync(cancellationToken);
    }
}
