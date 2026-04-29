using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Domain.Interfaces;

public interface IReportParameterRepository : IBaseRepository<ReportParameter>
{
    Task<IEnumerable<ReportParameter>> GetByLinkIdAsync(Guid linkId, CancellationToken cancellationToken = default);
    Task DeleteByLinkIdAsync(Guid linkId, CancellationToken cancellationToken = default);
}
