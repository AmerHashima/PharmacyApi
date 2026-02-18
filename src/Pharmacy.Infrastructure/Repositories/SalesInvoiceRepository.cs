using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for SalesInvoice entity
/// </summary>
public class SalesInvoiceRepository : BaseRepository<SalesInvoice>, ISalesInvoiceRepository
{
    public SalesInvoiceRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<SalesInvoice?> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Branch)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceStatus)
            .Include(i => i.Cashier)
            .Include(i => i.Items)
                .ThenInclude(item => item.Product)
            .Where(i => i.InvoiceNumber == invoiceNumber && !i.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<SalesInvoice>> GetByBranchAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Branch)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceStatus)
            .Include(i => i.Cashier)
            .Where(i => i.BranchId == branchId && !i.IsDeleted)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SalesInvoice>> GetByCashierAsync(Guid cashierId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Branch)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceStatus)
            .Include(i => i.Cashier)
            .Where(i => i.CashierId == cashierId && !i.IsDeleted)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SalesInvoice>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? branchId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(i => i.Branch)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceStatus)
            .Include(i => i.Cashier)
            .Where(i => i.InvoiceDate >= startDate && i.InvoiceDate <= endDate && !i.IsDeleted);
        
        if (branchId.HasValue)
        {
            query = query.Where(i => i.BranchId == branchId.Value);
        }
        
        return await query
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<SalesInvoice?> GetWithItemsAsync(Guid invoiceId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Branch)
            .Include(i => i.PaymentMethod)
            .Include(i => i.InvoiceStatus)
            .Include(i => i.Cashier)
            .Include(i => i.Items)
                .ThenInclude(item => item.Product)
            .Where(i => i.Oid == invoiceId && !i.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<string> GenerateInvoiceNumberAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.ToString("yyyyMMdd");
        var branchCode = await _context.Set<Branch>()
            .Where(b => b.Oid == branchId)
            .Select(b => b.BranchCode)
            .FirstOrDefaultAsync(cancellationToken) ?? "UNK";
        
        var lastInvoice = await _dbSet
            .Where(i => i.InvoiceNumber.StartsWith($"INV-{branchCode}-{today}"))
            .OrderByDescending(i => i.InvoiceNumber)
            .FirstOrDefaultAsync(cancellationToken);
        
        int sequence = 1;
        if (lastInvoice?.InvoiceNumber != null)
        {
            var parts = lastInvoice.InvoiceNumber.Split('-');
            if (parts.Length >= 4 && int.TryParse(parts[^1], out int lastSequence))
            {
                sequence = lastSequence + 1;
            }
        }
        
        return $"INV-{branchCode}-{today}-{sequence:D4}";
    }

    public async Task<decimal> GetDailySalesTotalAsync(Guid branchId, DateTime date, CancellationToken cancellationToken = default)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);
        
        return await _dbSet
            .Where(i => i.BranchId == branchId && 
                       i.InvoiceDate >= startOfDay && 
                       i.InvoiceDate < endOfDay && 
                       !i.IsDeleted)
            .SumAsync(i => i.TotalAmount ?? 0, cancellationToken);
    }
}
