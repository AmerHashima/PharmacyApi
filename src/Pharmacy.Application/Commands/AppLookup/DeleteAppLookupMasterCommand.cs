using MediatR;

namespace Pharmacy.Application.Commands.AppLookup;

/// <summary>
/// Command to delete an AppLookupMaster (soft delete)
/// </summary>
public record DeleteAppLookupMasterCommand(Guid Id) : IRequest<bool>;
