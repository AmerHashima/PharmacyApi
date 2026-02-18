using MediatR;

namespace Pharmacy.Application.Commands.AppLookup;

/// <summary>
/// Command to delete an AppLookupDetail (soft delete)
/// </summary>
public record DeleteAppLookupDetailCommand(Guid Id) : IRequest<bool>;
