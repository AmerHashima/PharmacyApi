using Pharmacy.Application.DTOs.Branch;
using Pharmacy.Application.DTOs.Common;
using MediatR;

namespace Pharmacy.Application.Queries.Branch;

/// <summary>
/// Query to get branches with advanced filtering, sorting, and pagination
/// </summary>
public record GetBranchDataQuery(QueryRequest Request) : IRequest<PagedResult<BranchDto>>;
