using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Customer?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
    Task<Customer?> GetByIdentityNumberAsync(string identityNumber, CancellationToken cancellationToken = default);
    Task<Customer?> GetDefaultWalkInAsync(CancellationToken cancellationToken = default);
}
