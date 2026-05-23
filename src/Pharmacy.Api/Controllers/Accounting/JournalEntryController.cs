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
public class JournalEntryController : BaseApiController
{
    private readonly IMediator _mediator;

    public JournalEntryController(IMediator mediator) => _mediator = mediator;

    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<JournalEntryDto>>>> Query([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetJournalEntryDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Journal entries retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<JournalEntryDto>>($"Error retrieving journal entries: {ex.Message}", 500);
        }
    }

    [HttpPost("master-query")]
    public async Task<ActionResult<ApiResponse<PagedResult<JournalEntryMasterDto>>>> MasterQuery([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetJournalEntryMasterQuery(request), cancellationToken);
            return SuccessResponse(result, "Journal entries retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<JournalEntryMasterDto>>($"Error retrieving journal entries: {ex.Message}", 500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<JournalEntryDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetJournalEntryByIdQuery(id), cancellationToken);
        if (result is null) return ErrorResponse<JournalEntryDto>("Journal entry not found", 404);
        return SuccessResponse(result, "Journal entry retrieved successfully");
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<JournalEntryDto>>> Create([FromBody] CreateJournalEntryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateJournalEntryCommand(dto), cancellationToken);
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Journal entry created successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<JournalEntryDto>($"Error creating journal entry: {ex.Message}", 500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<JournalEntryDto>>> Update([FromBody] UpdateJournalEntryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new UpdateJournalEntryCommand(dto), cancellationToken);
            return SuccessResponse(result, "Journal entry updated successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<JournalEntryDto>(ex.Message, 404); }
        catch (Exception ex) { return ErrorResponse<JournalEntryDto>($"Error updating journal entry: {ex.Message}", 500); }
    }

    [HttpPost("detail-report")]
    public async Task<ActionResult<ApiResponse<PagedResult<JournalEntryDetailReportDto>>>> DetailReport([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetJournalEntryDetailReportQuery(request), cancellationToken);
            return SuccessResponse(result, "Journal entry detail report retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<JournalEntryDetailReportDto>>($"Error retrieving journal entry detail report: {ex.Message}", 500);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeleteJournalEntryCommand(id), cancellationToken);
            if (!result) return ErrorResponse<bool>("Journal entry not found", 404);
            return SuccessResponse(result, "Journal entry deleted successfully");
        }
        catch (Exception ex) { return ErrorResponse<bool>($"Error deleting journal entry: {ex.Message}", 500); }
    }

    /// <summary>
    /// ميزان المراجعة — Trial Balance report.
    /// Returns opening balance, period movement, and closing balance per account
    /// for the given date range and branch, with full account tree hierarchy.
    /// </summary>
    [HttpPost("trial-balance")]
    public async Task<ActionResult<ApiResponse<TrialBalanceReportDto>>> TrialBalance(
        [FromBody] TrialBalanceRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetTrialBalanceQuery(request), cancellationToken);
            return SuccessResponse(result, "Trial balance retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<TrialBalanceReportDto>($"Error retrieving trial balance: {ex.Message}", 500);
        }
    }
}
