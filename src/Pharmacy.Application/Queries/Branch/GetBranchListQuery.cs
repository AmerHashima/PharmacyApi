using Pharmacy.Application.DTOs.Branch;
using MediatR;

namespace Pharmacy.Application.Queries.Branch;

/// <summary>
/// Query to get all branches
/// </summary>
public record GetBranchListQuery() : IRequest<IEnumerable<BranchDto>>;
