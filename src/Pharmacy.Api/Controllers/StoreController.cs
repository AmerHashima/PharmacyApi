using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.Store;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Store;
using Pharmacy.Application.Queries.Store;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing stores within branches
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class StoreController : BaseApiController
{
    private readonly IMediator _mediator;

    public StoreController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Query stores with advanced filtering, sorting, and pagination
    /// </summary>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<StoreDto>>>> GetStoreData([FromBody] QueryRequest request)
    {
        try
        {
            var result = await _mediator.Send(new GetStoreDataQuery(request));
            return SuccessResponse(result, "Store data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<StoreDto>>($"Error retrieving store data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get all stores for a specific branch
    /// </summary>
    [HttpGet("by-branch/{branchId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<StoreDto>>>> GetByBranch(Guid branchId)
    {
        var result = await _mediator.Send(new GetStoresByBranchQuery(branchId));
        return SuccessResponse(result, "Stores retrieved successfully");
    }

    /// <summary>
    /// Get store by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<StoreDto>>> GetStore(Guid id)
    {
        var result = await _mediator.Send(new GetStoreByIdQuery(id));
        if (result == null)
            return ErrorResponse<StoreDto>("Store not found", 404);

        return SuccessResponse(result, "Store retrieved successfully");
    }

    /// <summary>
    /// Create a new store
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<StoreDto>>> CreateStore([FromBody] CreateStoreDto dto)
    {
        try
        {
            var result = await _mediator.Send(new CreateStoreCommand(dto));
            return CreatedResponse(result, nameof(GetStore), new { id = result.Oid }, "Store created successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<StoreDto>(ex.Message, 404); }
        catch (InvalidOperationException ex) { return ErrorResponse<StoreDto>(ex.Message, 400); }
    }

    /// <summary>
    /// Update an existing store
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<StoreDto>>> UpdateStore(Guid id, [FromBody] UpdateStoreDto dto)
    {
        try
        {
            if (id != dto.Oid)
                return ErrorResponse<StoreDto>("Store ID mismatch", 400);

            var result = await _mediator.Send(new UpdateStoreCommand(dto));
            return SuccessResponse(result, "Store updated successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<StoreDto>(ex.Message, 404); }
        catch (InvalidOperationException ex) { return ErrorResponse<StoreDto>(ex.Message, 400); }
    }

    /// <summary>
    /// Delete a store (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteStore(Guid id)
    {
        var result = await _mediator.Send(new DeleteStoreCommand(id));
        if (!result)
            return ErrorResponse("Store not found", 404);

        return SuccessResponse("Store deleted successfully");
    }
}
