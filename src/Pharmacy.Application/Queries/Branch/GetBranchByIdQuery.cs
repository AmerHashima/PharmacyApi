using Pharmacy.Application.DTOs.Branch;
using MediatR;

namespace Pharmacy.Application.Queries.Branch;

/// <summary>
/// Query to get a branch by ID
/// </summary>
public record GetBranchByIdQuery(Guid Id) : IRequest<BranchDto?>;
