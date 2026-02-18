using Pharmacy.Application.DTOs.AppLookup;
using MediatR;

namespace Pharmacy.Application.Commands.AppLookup;

/// <summary>
/// Command to update an existing AppLookupMaster (header)
/// </summary>
public record UpdateAppLookupMasterCommand(UpdateAppLookupMasterDto LookupMaster) : IRequest<AppLookupMasterDto>;
