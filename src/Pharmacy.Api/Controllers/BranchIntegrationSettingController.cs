using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.BranchIntegrationSetting;
using Pharmacy.Application.DTOs.BranchIntegrationSetting;
using Pharmacy.Application.Queries.BranchIntegrationSetting;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing branch integration settings
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class BranchIntegrationSettingController : BaseApiController
{
    private readonly IMediator _mediator;

    public BranchIntegrationSettingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get integration settings by branch ID
    /// </summary>
    [HttpGet("branch/{branchId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BranchIntegrationSettingDto>>>> GetByBranch(Guid branchId)
    {
        var query = new GetBranchIntegrationSettingsByBranchQuery(branchId);
        var settings = await _mediator.Send(query);
        return SuccessResponse(settings, "Branch integration settings retrieved successfully");
    }

    /// <summary>
    /// Get integration setting by branch and provider
    /// </summary>
    [HttpGet("branch/{branchId}/provider/{providerId}")]
    public async Task<ActionResult<ApiResponse<BranchIntegrationSettingDto>>> GetByBranchAndProvider(Guid branchId, Guid providerId)
    {
        var query = new GetBranchIntegrationSettingByBranchAndProviderQuery(branchId, providerId);
        var setting = await _mediator.Send(query);

        if (setting == null)
            return ErrorResponse<BranchIntegrationSettingDto>("Integration setting not found", 404);

        return SuccessResponse(setting, "Integration setting retrieved successfully");
    }

    /// <summary>
    /// Create a new branch integration setting
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<BranchIntegrationSettingDto>>> CreateBranchIntegrationSetting([FromBody] CreateBranchIntegrationSettingDto dto)
    {
        try
        {
            var command = new CreateBranchIntegrationSettingCommand(dto);
            var setting = await _mediator.Send(command);
            return CreatedResponse(setting, nameof(GetByBranchAndProvider), new { branchId = setting.BranchId, providerId = setting.IntegrationProviderId }, "Integration setting created successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<BranchIntegrationSettingDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Update an existing branch integration setting
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<BranchIntegrationSettingDto>>> UpdateBranchIntegrationSetting(Guid id, [FromBody] UpdateBranchIntegrationSettingDto dto)
    {
        if (id != dto.Oid)
            return ErrorResponse<BranchIntegrationSettingDto>("ID mismatch", 400);

        try
        {
            var command = new UpdateBranchIntegrationSettingCommand(dto);
            var setting = await _mediator.Send(command);
            return SuccessResponse(setting, "Integration setting updated successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<BranchIntegrationSettingDto>(ex.Message, 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<BranchIntegrationSettingDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Delete a branch integration setting
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteBranchIntegrationSetting(Guid id)
    {
        try
        {
            var command = new DeleteBranchIntegrationSettingCommand(id);
            var result = await _mediator.Send(command);
            return SuccessResponse(result, "Integration setting deleted successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<bool>(ex.Message, 404);
        }
    }
}