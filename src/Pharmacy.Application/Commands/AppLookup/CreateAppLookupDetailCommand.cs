using Pharmacy.Application.DTOs.AppLookup;
using MediatR;

namespace Pharmacy.Application.Commands.AppLookup;

public record CreateAppLookupDetailCommand(CreateAppLookupDetailDto LookupDetail) : IRequest<AppLookupDetailDto>;