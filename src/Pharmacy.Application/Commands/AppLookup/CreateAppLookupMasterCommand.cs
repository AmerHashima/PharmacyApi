using Pharmacy.Application.DTOs.AppLookup;
using MediatR;

namespace Pharmacy.Application.Commands.AppLookup;

public record CreateAppLookupMasterCommand(CreateAppLookupMasterDto LookupMaster) : IRequest<AppLookupMasterDto>;