using MediatR;

namespace Pharmacy.Application.Commands.Branch;

/// <summary>
/// Command to delete a Branch (soft delete)
/// </summary>
public record DeleteBranchCommand(Guid Id) : IRequest<bool>;
