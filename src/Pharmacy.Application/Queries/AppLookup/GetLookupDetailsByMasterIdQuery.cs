using Pharmacy.Application.DTOs.AppLookup;
using MediatR;

namespace Pharmacy.Application.Queries.AppLookup;

public record GetLookupDetailsByMasterIdQuery(Guid MasterID) : IRequest<IEnumerable<AppLookupDetailDto>>;