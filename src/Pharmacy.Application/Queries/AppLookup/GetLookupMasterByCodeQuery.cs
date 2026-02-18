using Pharmacy.Application.DTOs.AppLookup;
using MediatR;

namespace Pharmacy.Application.Queries.AppLookup;

public record GetLookupMasterByCodeQuery(string LookupCode, bool IncludeDetails = true) : IRequest<AppLookupMasterDto?>;