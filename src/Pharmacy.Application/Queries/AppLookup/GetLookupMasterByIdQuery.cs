using Pharmacy.Application.DTOs.AppLookup;
using MediatR;

namespace Pharmacy.Application.Queries.AppLookup;

public record GetLookupMasterByIdQuery(Guid Id, bool IncludeDetails = true) : IRequest<AppLookupMasterDto?>;