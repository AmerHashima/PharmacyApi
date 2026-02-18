using Pharmacy.Application.DTOs.Stakeholder;
using MediatR;

namespace Pharmacy.Application.Queries.Stakeholder;

/// <summary>
/// Query to get all stakeholders with optional type filter
/// </summary>
public record GetStakeholderListQuery(Guid? StakeholderTypeId = null) : IRequest<IEnumerable<StakeholderDto>>;
