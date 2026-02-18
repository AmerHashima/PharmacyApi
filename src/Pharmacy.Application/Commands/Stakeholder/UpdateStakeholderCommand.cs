using Pharmacy.Application.DTOs.Stakeholder;
using MediatR;

namespace Pharmacy.Application.Commands.Stakeholder;

/// <summary>
/// Command to update an existing Stakeholder
/// </summary>
public record UpdateStakeholderCommand(UpdateStakeholderDto Stakeholder) : IRequest<StakeholderDto>;
