using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IGenericNameRepository : IBaseRepository<GenericName>
{
    Task<IEnumerable<GenericName>> SearchAsync(string term, CancellationToken cancellationToken = default);
}
