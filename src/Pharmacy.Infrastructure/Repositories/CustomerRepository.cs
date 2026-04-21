using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(PharmacyDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.Phone == phone && !c.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Customer?> GetByIdentityNumberAsync(string identityNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.IdentityNumber == identityNumber && !c.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Customer?> GetDefaultWalkInAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.IsWalkIn && !c.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
