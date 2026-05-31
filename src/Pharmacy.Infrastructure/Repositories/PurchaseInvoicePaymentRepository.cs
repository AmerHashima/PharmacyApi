using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class PurchaseInvoicePaymentRepository : BaseRepository<PurchaseInvoicePayment>, IPurchaseInvoicePaymentRepository
{
    public PurchaseInvoicePaymentRepository(PharmacyDbContext context) : base(context) { }

    public async Task<IEnumerable<PurchaseInvoicePayment>> GetByPurchaseInvoiceAsync(Guid purchaseInvoiceId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.PaymentMethod)
            .Include(x => x.PaymentVoucher)
            .Where(x => x.PurchaseInvoiceId == purchaseInvoiceId && !x.IsDeleted)
            .OrderBy(x => x.PaymentDate)
            .ToListAsync(cancellationToken);
    }
}
