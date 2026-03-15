using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for ReturnInvoice entity
/// </summary>
public class ReturnInvoiceRepository : BaseRepository<ReturnInvoice>, IReturnInvoiceRepository
{
    public ReturnInvoiceRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<ReturnInvoice?> GetByReturnNumberAsync(string returnNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.OriginalInvoice)
            .Include(i => i.Branch)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceStatus)
            .Include(i => i.Cashier)
            .Include(i => i.ReturnReason)
            .Include(i => i.Items)
                .ThenInclude(item => item.Product)
            .Where(i => i.ReturnNumber == returnNumber && !i.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<ReturnInvoice>> GetByOriginalInvoiceAsync(Guid originalInvoiceId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.OriginalInvoice)
            .Include(i => i.Branch)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceStatus)
            .Include(i => i.Cashier)
            .Include(i => i.ReturnReason)
            .Where(i => i.OriginalInvoiceId == originalInvoiceId && !i.IsDeleted)
            .OrderByDescending(i => i.ReturnDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ReturnInvoice>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.OriginalInvoice)
            .Include(i => i.Branch)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceStatus)
            .Include(i => i.Cashier)
            .Include(i => i.ReturnReason)
            .Where(i => i.BranchId == branchId && !i.IsDeleted)
            .OrderByDescending(i => i.ReturnDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ReturnInvoice>> GetByCashierAsync(Guid cashierId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.OriginalInvoice)
            .Include(i => i.Branch)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceStatus)
            .Include(i => i.Cashier)
            .Include(i => i.ReturnReason)
            .Where(i => i.CashierId == cashierId && !i.IsDeleted)
            .OrderByDescending(i => i.ReturnDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ReturnInvoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? branchId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(i => i.OriginalInvoice)
            .Include(i => i.Branch)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceStatus)
            .Include(i => i.Cashier)
            .Include(i => i.ReturnReason)
            .Where(i => i.ReturnDate >= startDate && i.ReturnDate <= endDate && !i.IsDeleted);

        if (branchId.HasValue)
        {
            query = query.Where(i => i.BranchId == branchId.Value);
        }

        return await query
            .OrderByDescending(i => i.ReturnDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<ReturnInvoice?> GetWithItemsAsync(Guid returnInvoiceId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.OriginalInvoice)
            .Include(i => i.Branch)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceStatus)
            .Include(i => i.Cashier)
            .Include(i => i.ReturnReason)
            .Include(i => i.Items)
                .ThenInclude(item => item.Product)
            .Where(i => i.Oid == returnInvoiceId && !i.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string> GenerateReturnNumberAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.ToString("yyyyMMdd");
        var branchCode = await _context.Set<Branch>()
            .Where(b => b.Oid == branchId)
            .Select(b => b.BranchCode)
            .FirstOrDefaultAsync(cancellationToken) ?? "UNK";

        var lastReturn = await _dbSet
            .Where(i => i.ReturnNumber.StartsWith($"RET-{branchCode}-{today}"))
            .OrderByDescending(i => i.ReturnNumber)
            .FirstOrDefaultAsync(cancellationToken);

        int sequence = 1;
        if (lastReturn?.ReturnNumber != null)
        {
            var parts = lastReturn.ReturnNumber.Split('-');
            if (parts.Length >= 4 && int.TryParse(parts[^1], out int lastSequence))
            {
                sequence = lastSequence + 1;
            }
        }

        return $"RET-{branchCode}-{today}-{sequence:D4}";
    }
}
