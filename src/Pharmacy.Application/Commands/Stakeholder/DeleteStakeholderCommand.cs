using MediatR;

namespace Pharmacy.Application.Commands.Stakeholder;

/// <summary>
/// Command to delete a Stakeholder (soft delete)
/// </summary>
public record DeleteStakeholderCommand(Guid Id) : IRequest<bool>;
