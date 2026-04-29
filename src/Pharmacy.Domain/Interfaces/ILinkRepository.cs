using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface ILinkRepository : IBaseRepository<Link>
{
    Task<Link?> GetWithParametersAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Link>> GetActiveLinksAsync(CancellationToken cancellationToken = default);
}
