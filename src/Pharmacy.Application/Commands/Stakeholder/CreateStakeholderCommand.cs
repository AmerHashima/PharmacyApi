using Pharmacy.Application.DTOs.Stakeholder;
using MediatR;

namespace Pharmacy.Application.Commands.Stakeholder;

/// <summary>
/// Command to create a new Stakeholder
/// </summary>
public record CreateStakeholderCommand(CreateStakeholderDto Stakeholder) : IRequest<StakeholderDto>;
