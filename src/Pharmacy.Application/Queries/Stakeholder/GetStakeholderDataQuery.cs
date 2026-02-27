using Pharmacy.Application.DTOs.Stakeholder;
using Pharmacy.Application.DTOs.Common;
using MediatR;

namespace Pharmacy.Application.Queries.Stakeholder;

/// <summary>
/// Query to get stakeholders with advanced filtering, sorting, and pagination
/// </summary>
public record GetStakeholderDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<StakeholderDto>>;
