using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class SalesInvoicePaymentRepository : BaseRepository<SalesInvoicePayment>, ISalesInvoicePaymentRepository
{
    public SalesInvoicePaymentRepository(PharmacyDbContext context) : base(context) { }

    public async Task<IEnumerable<SalesInvoicePayment>> GetBySalesInvoiceAsync(Guid salesInvoiceId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.PaymentMethod)
            .Include(x => x.Shift)
            .Where(x => x.SalesInvoiceId == salesInvoiceId && !x.IsDeleted)
            .OrderBy(x => x.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SalesInvoicePayment>> GetByShiftAsync(Guid shiftId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.PaymentMethod)
            .Include(x => x.SalesInvoice)
            .Where(x => x.ShiftId == shiftId && !x.IsDeleted)
            .OrderBy(x => x.PaymentDate)
            .ToListAsync(cancellationToken);
    }
}
