using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class CashierShiftRepository : BaseRepository<CashierShift>, ICashierShiftRepository
{
    public CashierShiftRepository(PharmacyDbContext context) : base(context) { }

    public async Task<CashierShift?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.Branch)
            .Include(x => x.CashBox)
            .Include(x => x.User)
            .Include(x => x.Details)
                .ThenInclude(d => d.TransactionType)
            .Include(x => x.Details)
                .ThenInclude(d => d.PaymentMethod)
            .FirstOrDefaultAsync(x => x.Oid == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<CashierShift>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.User)
            .Include(x => x.CashBox)
            .Where(x => x.BranchId == branchId && !x.IsDeleted)
            .OrderByDescending(x => x.OpenDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CashierShift>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.Branch)
            .Include(x => x.CashBox)
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .OrderByDescending(x => x.OpenDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<CashierShift?> GetOpenShiftAsync(Guid branchId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.CashBox)
            .Where(x => x.BranchId == branchId && x.UserId == userId
                     && x.CloseDate == null && x.Status == 1 && !x.IsDeleted)
            .OrderByDescending(x => x.OpenDate)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
