using Pharmacy.Application.DTOs.Stakeholder;
using MediatR;

namespace Pharmacy.Application.Queries.Stakeholder;

/// <summary>
/// Query to get a stakeholder by ID
/// </summary>
public record GetStakeholderByIdQuery(Guid Id) : IRequest<StakeholderDto?>;
