using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.Stakeholder;
using Pharmacy.Application.DTOs.Stakeholder;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Stakeholder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing stakeholders (Pharmacies, Suppliers, etc.)
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class StakeholderController : BaseApiController
{
    private readonly IMediator _mediator;

    public StakeholderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get stakeholder data with advanced filtering, sorting, and pagination
    /// </summary>
    /// <param name="request">Query request with filters, sorting, and pagination</param>
    /// <returns>Paginated stakeholder data</returns>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<StakeholderDto>>>> GetStakeholderData([FromBody] QueryRequest request)
    {
        try
        {
            var query = new GetStakeholderDataQuery(request);
            var result = await _mediator.Send(query);
            return SuccessResponse(result, "Stakeholder data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<StakeholderDto>>($"Error retrieving stakeholder data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get all stakeholders with optional type filter
    /// </summary>
    /// <param name="stakeholderTypeId">Optional: Filter by stakeholder type</param>
    /// <returns>List of stakeholders</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<StakeholderDto>>>> GetStakeholders([FromQuery] Guid? stakeholderTypeId = null)
    {
        var query = new GetStakeholderListQuery(stakeholderTypeId);
        var stakeholders = await _mediator.Send(query);
        return SuccessResponse(stakeholders, "Stakeholders retrieved successfully");
    }

    /// <summary>
    /// Get stakeholder by ID
    /// </summary>
    /// <param name="id">Stakeholder ID</param>
    /// <returns>Stakeholder details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<StakeholderDto>>> GetStakeholder(Guid id)
    {
        var query = new GetStakeholderByIdQuery(id);
        var stakeholder = await _mediator.Send(query);

        if (stakeholder == null)
            return ErrorResponse<StakeholderDto>("Stakeholder not found", 404);

        return SuccessResponse(stakeholder, "Stakeholder retrieved successfully");
    }

    /// <summary>
    /// Create a new stakeholder
    /// </summary>
    /// <param name="createStakeholderDto">Stakeholder creation data</param>
    /// <returns>Created stakeholder</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<StakeholderDto>>> CreateStakeholder([FromBody] CreateStakeholderDto createStakeholderDto)
    {
        try
        {
            var command = new CreateStakeholderCommand(createStakeholderDto);
            var stakeholder = await _mediator.Send(command);
            return CreatedResponse(stakeholder, nameof(GetStakeholder), new { id = stakeholder.Oid }, "Stakeholder created successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<StakeholderDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Update an existing stakeholder
    /// </summary>
    /// <param name="id">Stakeholder ID</param>
    /// <param name="updateStakeholderDto">Stakeholder update data</param>
    /// <returns>Updated stakeholder</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<StakeholderDto>>> UpdateStakeholder(Guid id, [FromBody] UpdateStakeholderDto updateStakeholderDto)
    {
        try
        {
            if (id != updateStakeholderDto.Oid)
                return ErrorResponse<StakeholderDto>("Stakeholder ID mismatch", 400);

            var command = new UpdateStakeholderCommand(updateStakeholderDto);
            var stakeholder = await _mediator.Send(command);
            return SuccessResponse(stakeholder, "Stakeholder updated successfully");
        }
        catch (KeyNotFoundException)
        {
            return ErrorResponse<StakeholderDto>("Stakeholder not found", 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<StakeholderDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Delete a stakeholder (soft delete)
    /// </summary>
    /// <param name="id">Stakeholder ID</param>
    /// <returns>Success response</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteStakeholder(Guid id)
    {
        var command = new DeleteStakeholderCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
            return ErrorResponse("Stakeholder not found", 404);

        return SuccessResponse("Stakeholder deleted successfully");
    }
}
