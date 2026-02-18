using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.AppLookup;
using Pharmacy.Application.DTOs.AppLookup;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.AppLookup;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing AppLookup Master (header) and Detail entities
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class AppLookupController : BaseApiController
{
    private readonly IMediator _mediator;

    public AppLookupController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Master (Header) Endpoints

    /// <summary>
    /// Get lookup data with advanced filtering, sorting, and pagination
    /// </summary>
    /// <param name="request">Query request with filters, sorting, and pagination</param>
    /// <returns>Paginated lookup data</returns>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<AppLookupMasterDto>>>> GetLookupData([FromBody] QueryRequest request)
    {
        try
        {
            var query = new GetLookupDataQuery(request);
            var result = await _mediator.Send(query);
            return SuccessResponse(result, "Lookup data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<AppLookupMasterDto>>($"Error retrieving lookup data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get lookup master by ID
    /// </summary>
    /// <param name="id">Lookup master ID</param>
    /// <param name="includeDetails">Include lookup details</param>
    /// <returns>Lookup master with details</returns>
    [HttpGet("masters/{id:guid}")]
    public async Task<ActionResult<ApiResponse<AppLookupMasterDto>>> GetLookupMasterById(
        Guid id,
        [FromQuery] bool includeDetails = true)
    {
        var query = new GetLookupMasterByIdQuery(id, includeDetails);
        var lookup = await _mediator.Send(query);

        if (lookup == null)
            return ErrorResponse<AppLookupMasterDto>("Lookup master not found", 404);

        return SuccessResponse(lookup, "Lookup master retrieved successfully");
    }

    /// <summary>
    /// Get lookup master with details by code
    /// </summary>
    /// <param name="lookupCode">Lookup code (e.g., GENDER, MARITAL_STATUS)</param>
    /// <param name="includeDetails">Include lookup details</param>
    /// <returns>Lookup master with details</returns>
    [HttpGet("masters/code/{lookupCode}")]
    public async Task<ActionResult<ApiResponse<AppLookupMasterDto>>> GetLookupByCode(
        string lookupCode,
        [FromQuery] bool includeDetails = true)
    {
        var query = new GetLookupMasterByCodeQuery(lookupCode, includeDetails);
        var lookup = await _mediator.Send(query);

        if (lookup == null)
            return ErrorResponse<AppLookupMasterDto>("Lookup not found", 404);

        return SuccessResponse(lookup, "Lookup retrieved successfully");
    }

    /// <summary>
    /// Create a new lookup master (header)
    /// </summary>
    /// <param name="createDto">Lookup master creation data</param>
    /// <returns>Created lookup master</returns>
    [HttpPost("masters")]
    public async Task<ActionResult<ApiResponse<AppLookupMasterDto>>> CreateLookupMaster([FromBody] CreateAppLookupMasterDto createDto)
    {
        try
        {
            var command = new CreateAppLookupMasterCommand(createDto);
            var lookupMaster = await _mediator.Send(command);
            return CreatedResponse(lookupMaster, nameof(GetLookupMasterById), new { id = lookupMaster.Oid }, "Lookup master created successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<AppLookupMasterDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Update an existing lookup master (header)
    /// </summary>
    /// <param name="id">Lookup master ID</param>
    /// <param name="updateDto">Lookup master update data</param>
    /// <returns>Updated lookup master</returns>
    [HttpPut("masters/{id:guid}")]
    public async Task<ActionResult<ApiResponse<AppLookupMasterDto>>> UpdateLookupMaster(Guid id, [FromBody] UpdateAppLookupMasterDto updateDto)
    {
        try
        {
            if (id != updateDto.Oid)
                return ErrorResponse<AppLookupMasterDto>("ID mismatch", 400);

            var command = new UpdateAppLookupMasterCommand(updateDto);
            var lookupMaster = await _mediator.Send(command);
            return SuccessResponse(lookupMaster, "Lookup master updated successfully");
        }
        catch (KeyNotFoundException)
        {
            return ErrorResponse<AppLookupMasterDto>("Lookup master not found", 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<AppLookupMasterDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Delete a lookup master (soft delete) - also deletes associated details
    /// </summary>
    /// <param name="id">Lookup master ID</param>
    /// <returns>Success response</returns>
    [HttpDelete("masters/{id:guid}")]
    public async Task<ActionResult<ApiResponse>> DeleteLookupMaster(Guid id)
    {
        try
        {
            var command = new DeleteAppLookupMasterCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
                return ErrorResponse("Lookup master not found", 404);

            return SuccessResponse("Lookup master deleted successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse(ex.Message, 400);
        }
    }

    #endregion

    #region Detail Endpoints

    /// <summary>
    /// Get lookup details by master ID
    /// </summary>
    /// <param name="masterId">Master lookup ID</param>
    /// <returns>List of lookup details</returns>
    [HttpGet("masters/{masterId:guid}/details")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AppLookupDetailDto>>>> GetLookupDetails(Guid masterId)
    {
        var query = new GetLookupDetailsByMasterIdQuery(masterId);
        var details = await _mediator.Send(query);
        return SuccessResponse(details, "Lookup details retrieved successfully");
    }

    /// <summary>
    /// Create a new lookup detail
    /// </summary>
    /// <param name="createDto">Lookup detail creation data</param>
    /// <returns>Created lookup detail</returns>
    [HttpPost("details")]
    public async Task<ActionResult<ApiResponse<AppLookupDetailDto>>> CreateLookupDetail([FromBody] CreateAppLookupDetailDto createDto)
    {
        try
        {
            var command = new CreateAppLookupDetailCommand(createDto);
            var lookupDetail = await _mediator.Send(command);
            return CreatedResponse(lookupDetail, nameof(GetLookupDetails), new { masterId = lookupDetail.LookupMasterID }, "Lookup detail created successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<AppLookupDetailDto>(ex.Message, 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<AppLookupDetailDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Update an existing lookup detail
    /// </summary>
    /// <param name="id">Lookup detail ID</param>
    /// <param name="updateDto">Lookup detail update data</param>
    /// <returns>Updated lookup detail</returns>
    [HttpPut("details/{id:guid}")]
    public async Task<ActionResult<ApiResponse<AppLookupDetailDto>>> UpdateLookupDetail(Guid id, [FromBody] UpdateAppLookupDetailDto updateDto)
    {
        try
        {
            if (id != updateDto.Oid)
                return ErrorResponse<AppLookupDetailDto>("ID mismatch", 400);

            var command = new UpdateAppLookupDetailCommand(updateDto);
            var lookupDetail = await _mediator.Send(command);
            return SuccessResponse(lookupDetail, "Lookup detail updated successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<AppLookupDetailDto>(ex.Message, 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<AppLookupDetailDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Delete a lookup detail (soft delete)
    /// </summary>
    /// <param name="id">Lookup detail ID</param>
    /// <returns>Success response</returns>
    [HttpDelete("details/{id:guid}")]
    public async Task<ActionResult<ApiResponse>> DeleteLookupDetail(Guid id)
    {
        var command = new DeleteAppLookupDetailCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
            return ErrorResponse("Lookup detail not found", 404);

        return SuccessResponse("Lookup detail deleted successfully");
    }

    #endregion

    #region Legacy Endpoints (for backward compatibility)

    /// <summary>
    /// Get lookup master with details by code (legacy endpoint)
    /// </summary>
    [HttpGet("{lookupCode}")]
    public async Task<ActionResult<ApiResponse<AppLookupMasterDto>>> GetLookupByCodeLegacy(
        string lookupCode,
        [FromQuery] bool includeDetails = true)
    {
        return await GetLookupByCode(lookupCode, includeDetails);
    }

    /// <summary>
    /// Get lookup details by master ID (legacy endpoint)
    /// </summary>
    [HttpGet("{masterID:guid}/details")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AppLookupDetailDto>>>> GetLookupDetailsLegacy(Guid masterID)
    {
        return await GetLookupDetails(masterID);
    }

    #endregion
}