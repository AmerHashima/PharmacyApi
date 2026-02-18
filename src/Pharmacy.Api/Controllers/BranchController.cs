using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.Branch;
using Pharmacy.Application.DTOs.Branch;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Branch;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing pharmacy branches
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class BranchController : BaseApiController
{
    private readonly IMediator _mediator;

    public BranchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get branch data with advanced filtering, sorting, and pagination
    /// </summary>
    /// <param name="request">Query request with filters, sorting, and pagination</param>
    /// <returns>Paginated branch data</returns>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<BranchDto>>>> GetBranchData([FromBody] QueryRequest request)
    {
        try
        {
            var query = new GetBranchDataQuery(request);
            var result = await _mediator.Send(query);
            return SuccessResponse(result, "Branch data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<BranchDto>>($"Error retrieving branch data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get all branches
    /// </summary>
    /// <returns>List of branches</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BranchDto>>>> GetBranches()
    {
        var query = new GetBranchListQuery();
        var branches = await _mediator.Send(query);
        return SuccessResponse(branches, "Branches retrieved successfully");
    }

    /// <summary>
    /// Get branch by ID
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <returns>Branch details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BranchDto>>> GetBranch(Guid id)
    {
        var query = new GetBranchByIdQuery(id);
        var branch = await _mediator.Send(query);

        if (branch == null)
            return ErrorResponse<BranchDto>("Branch not found", 404);

        return SuccessResponse(branch, "Branch retrieved successfully");
    }

    /// <summary>
    /// Create a new branch
    /// </summary>
    /// <param name="createBranchDto">Branch creation data</param>
    /// <returns>Created branch</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<BranchDto>>> CreateBranch([FromBody] CreateBranchDto createBranchDto)
    {
        try
        {
            var command = new CreateBranchCommand(createBranchDto);
            var branch = await _mediator.Send(command);
            return CreatedResponse(branch, nameof(GetBranch), new { id = branch.Oid }, "Branch created successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<BranchDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Update an existing branch
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <param name="updateBranchDto">Branch update data</param>
    /// <returns>Updated branch</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<BranchDto>>> UpdateBranch(Guid id, [FromBody] UpdateBranchDto updateBranchDto)
    {
        try
        {
            if (id != updateBranchDto.Oid)
                return ErrorResponse<BranchDto>("Branch ID mismatch", 400);

            var command = new UpdateBranchCommand(updateBranchDto);
            var branch = await _mediator.Send(command);
            return SuccessResponse(branch, "Branch updated successfully");
        }
        catch (KeyNotFoundException)
        {
            return ErrorResponse<BranchDto>("Branch not found", 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<BranchDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Delete a branch (soft delete)
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <returns>Success response</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteBranch(Guid id)
    {
        var command = new DeleteBranchCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
            return ErrorResponse("Branch not found", 404);

        return SuccessResponse("Branch deleted successfully");
    }
}
