using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Accounting;

namespace Pharmacy.Api.Controllers.Accounting;

[Route("api/accounting/[controller]")]
[Authorize]
public class AccountingSettingsController : BaseApiController
{
    private readonly IMediator _mediator;

    public AccountingSettingsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<AccountingSettingsDto>>>> Query(
        [FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetAccountingSettingsDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Accounting settings retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<AccountingSettingsDto>>($"Error retrieving accounting settings: {ex.Message}", 500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<AccountingSettingsDto>>> GetById(
        Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAccountingSettingsByIdQuery(id), cancellationToken);
        if (result is null) return ErrorResponse<AccountingSettingsDto>("Accounting settings not found", 404);
        return SuccessResponse(result, "Accounting settings retrieved successfully");
    }

    [HttpGet("branch/{branchId}")]
    public async Task<ActionResult<ApiResponse<AccountingSettingsDto>>> GetByBranch(
        Guid branchId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAccountingSettingsByBranchQuery(branchId), cancellationToken);
        if (result is null) return ErrorResponse<AccountingSettingsDto>("Accounting settings not found for this branch", 404);
        return SuccessResponse(result, "Accounting settings retrieved successfully");
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<AccountingSettingsDto>>> Create(
        [FromBody] CreateAccountingSettingsDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateAccountingSettingsCommand(dto), cancellationToken);
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Accounting settings created successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<AccountingSettingsDto>(ex.Message, 409);
        }
        catch (Exception ex)
        {
            return ErrorResponse<AccountingSettingsDto>($"Error creating accounting settings: {ex.Message}", 500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<AccountingSettingsDto>>> Update(
        [FromBody] UpdateAccountingSettingsDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new UpdateAccountingSettingsCommand(dto), cancellationToken);
            return SuccessResponse(result, "Accounting settings updated successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<AccountingSettingsDto>(ex.Message, 404); }
        catch (Exception ex) { return ErrorResponse<AccountingSettingsDto>($"Error updating accounting settings: {ex.Message}", 500); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(
        Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeleteAccountingSettingsCommand(id), cancellationToken);
            if (!result) return ErrorResponse<bool>("Accounting settings not found", 404);
            return SuccessResponse(result, "Accounting settings deleted successfully");
        }
        catch (Exception ex) { return ErrorResponse<bool>($"Error deleting accounting settings: {ex.Message}", 500); }
    }
}
