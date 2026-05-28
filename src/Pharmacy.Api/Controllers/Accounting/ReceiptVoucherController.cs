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
public class ReceiptVoucherController : BaseApiController
{
    private readonly IMediator _mediator;

    public ReceiptVoucherController(IMediator mediator) => _mediator = mediator;

    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<ReceiptVoucherDto>>>> Query([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetReceiptVoucherDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Receipt vouchers retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<ReceiptVoucherDto>>($"Error retrieving receipt vouchers: {ex.Message}", 500);
        }
    }

    [HttpPost("master-query")]
    public async Task<ActionResult<ApiResponse<PagedResult<ReceiptVoucherMasterDto>>>> MasterQuery([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetReceiptVoucherMasterQuery(request), cancellationToken);
            return SuccessResponse(result, "Receipt vouchers retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<ReceiptVoucherMasterDto>>($"Error retrieving receipt vouchers: {ex.Message}", 500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ReceiptVoucherDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetReceiptVoucherByIdQuery(id), cancellationToken);
        if (result is null) return ErrorResponse<ReceiptVoucherDto>("Receipt voucher not found", 404);
        return SuccessResponse(result, "Receipt voucher retrieved successfully");
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ReceiptVoucherDto>>> Create([FromBody] CreateReceiptVoucherDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateReceiptVoucherCommand(dto), cancellationToken);
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Receipt voucher created successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<ReceiptVoucherDto>($"Error creating receipt voucher: {ex.Message}", 500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<ReceiptVoucherDto>>> Update([FromBody] UpdateReceiptVoucherDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new UpdateReceiptVoucherCommand(dto), cancellationToken);
            return SuccessResponse(result, "Receipt voucher updated successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<ReceiptVoucherDto>(ex.Message, 404); }
        catch (Exception ex) { return ErrorResponse<ReceiptVoucherDto>($"Error updating receipt voucher: {ex.Message}", 500); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeleteReceiptVoucherCommand(id), cancellationToken);
            if (!result) return ErrorResponse<bool>("Receipt voucher not found", 404);
            return SuccessResponse(result, "Receipt voucher deleted successfully");
        }
        catch (Exception ex) { return ErrorResponse<bool>($"Error deleting receipt voucher: {ex.Message}", 500); }
    }

    /// <summary>
    /// Re-create and link journal entries for a list of receipt vouchers that currently have no journal entry.
    /// </summary>
    [HttpPost("post-journal")]
    public async Task<ActionResult<ApiResponse<PostJournalBatchResultDto>>> PostJournal(
        [FromBody] List<Guid> ids, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new PostReceiptVoucherJournalCommand(ids), cancellationToken);
            var message = result.TotalFailed == 0
                ? $"All {result.TotalSucceeded} receipt voucher(s) posted successfully"
                : $"{result.TotalSucceeded} succeeded, {result.TotalFailed} failed";
            return SuccessResponse(result, message);
        }
        catch (Exception ex) { return ErrorResponse<PostJournalBatchResultDto>($"Error posting journals: {ex.Message}", 500); }
    }
}
