using Pharmacy.Application.DTOs.AppLookup;
using MediatR;

namespace Pharmacy.Application.Commands.AppLookup;

/// <summary>
/// Command to update an existing AppLookupDetail
/// </summary>
public record UpdateAppLookupDetailCommand(UpdateAppLookupDetailDto LookupDetail) : IRequest<AppLookupDetailDto>;
