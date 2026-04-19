using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class InvoiceSetupRepository : BaseRepository<InvoiceSetup>, IInvoiceSetupRepository
{
    public InvoiceSetupRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<InvoiceSetup>> GetByBranchIdAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.BranchId == branchId && !x.IsDeleted)
            .OrderBy(x => x.NameEn)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<InvoiceSetup>> GetGlobalTemplatesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.BranchId == null && !x.IsDeleted)
            .OrderBy(x => x.NameEn)
            .ToListAsync(cancellationToken);
    }

    public async Task<InvoiceSetup?> GetByBranchAndFormatAsync(Guid branchId, string format, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(x => x.BranchId == branchId && x.Format == format && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
