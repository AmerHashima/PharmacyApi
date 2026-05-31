using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class PurchaseInvoiceRepository : BaseRepository<PurchaseInvoice>, IPurchaseInvoiceRepository
{
    public PurchaseInvoiceRepository(PharmacyDbContext context) : base(context) { }

    public async Task<PurchaseInvoice?> GetWithPaymentsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.Branch)
            .Include(x => x.Supplier)
            .Include(x => x.FiscalYear)
            .Include(x => x.InvoiceStatus)
            .Include(x => x.StockTransaction)
            .Include(x => x.JournalEntry)
            .Include(x => x.Payments)
                .ThenInclude(p => p.PaymentMethod)
            .FirstOrDefaultAsync(x => x.Oid == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<PurchaseInvoice>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.Supplier)
            .Include(x => x.InvoiceStatus)
            .Where(x => x.BranchId == branchId && !x.IsDeleted)
            .OrderByDescending(x => x.PurchaseDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PurchaseInvoice>> GetBySupplierAsync(Guid supplierId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.Branch)
            .Include(x => x.InvoiceStatus)
            .Where(x => x.SupplierId == supplierId && !x.IsDeleted)
            .OrderByDescending(x => x.PurchaseDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PurchaseInvoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? branchId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(x => x.Supplier)
            .Include(x => x.Branch)
            .Where(x => x.PurchaseDate >= startDate && x.PurchaseDate <= endDate && !x.IsDeleted);

        if (branchId.HasValue)
            query = query.Where(x => x.BranchId == branchId.Value);

        return await query.OrderByDescending(x => x.PurchaseDate).ToListAsync(cancellationToken);
    }

    public async Task<PurchaseInvoice?> GetByInvoiceNumberAsync(string number, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.Supplier)
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.PurchaseInvoiceNumber == number && !x.IsDeleted, cancellationToken);
    }
}
