using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.CashierShift;
using Pharmacy.Application.DTOs.CashierShift;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.CashierShift;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Manages cashier shifts — open, close, and transaction detail lines.
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class CashierShiftController : BaseApiController
{
    private readonly IMediator _mediator;

    public CashierShiftController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ─────────────────────────────────────────────────────────────────────
    // Queries
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>Get a cashier shift by ID with all detail lines.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CashierShiftWithDetailsDto>>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetCashierShiftByIdQuery(id));
        if (result == null)
            return ErrorResponse<CashierShiftWithDetailsDto>("Cashier shift not found.", 404);
        return SuccessResponse(result);
    }

    /// <summary>Advanced filter / sort / paginate cashier shifts.</summary>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<CashierShiftDto>>>> Query([FromBody] QueryRequest request)
    {
        try
        {
            var result = await _mediator.Send(new GetCashierShiftDataQuery(request));
            return SuccessResponse(result);
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<CashierShiftDto>>(ex.Message, 500);
        }
    }

    /// <summary>Get all shifts for a branch.</summary>
    [HttpGet("branch/{branchId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CashierShiftDto>>>> GetByBranch(Guid branchId)
    {
        var result = await _mediator.Send(new GetCashierShiftsByBranchQuery(branchId));
        return SuccessResponse(result);
    }

    /// <summary>Get all shifts for a user.</summary>
    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CashierShiftDto>>>> GetByUser(Guid userId)
    {
        var result = await _mediator.Send(new GetCashierShiftsByUserQuery(userId));
        return SuccessResponse(result);
    }

    /// <summary>Get the currently open shift for a branch + user combination.</summary>
    [HttpGet("open")]
    public async Task<ActionResult<ApiResponse<CashierShiftWithDetailsDto?>>> GetOpenShift(
        [FromQuery] Guid branchId, [FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetOpenShiftQuery(branchId, userId));
        return SuccessResponse(result, result == null ? "No open shift found." : "Open shift retrieved.");
    }

    // ─────────────────────────────────────────────────────────────────────
    // Open / Close
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>Open a new cashier shift.</summary>
    [HttpPost("open")]
    public async Task<ActionResult<ApiResponse<CashierShiftWithDetailsDto>>> Open([FromBody] OpenCashierShiftDto dto)
    {
        try
        {
            var result = await _mediator.Send(new OpenCashierShiftCommand(dto));
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Cashier shift opened successfully.");
        }
        catch (InvalidOperationException ex) { return ErrorResponse<CashierShiftWithDetailsDto>(ex.Message, 400); }
        catch (KeyNotFoundException ex) { return ErrorResponse<CashierShiftWithDetailsDto>(ex.Message, 404); }
    }

    /// <summary>Close an open cashier shift and calculate balances.</summary>
    [HttpPost("{id:guid}/close")]
    public async Task<ActionResult<ApiResponse<CashierShiftWithDetailsDto>>> Close(Guid id, [FromBody] CloseCashierShiftDto dto)
    {
        try
        {
            var result = await _mediator.Send(new CloseCashierShiftCommand(id, dto));
            return SuccessResponse(result, "Cashier shift closed successfully.");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<CashierShiftWithDetailsDto>(ex.Message, 404); }
        catch (InvalidOperationException ex) { return ErrorResponse<CashierShiftWithDetailsDto>(ex.Message, 400); }
    }

    // ─────────────────────────────────────────────────────────────────────
    // Detail lines
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>Add a transaction detail line to an open shift.</summary>
    [HttpPost("details")]
    public async Task<ActionResult<ApiResponse<CashierShiftDetailDto>>> AddDetail([FromBody] AddCashierShiftDetailDto dto)
    {
        try
        {
            var result = await _mediator.Send(new AddCashierShiftDetailCommand(dto));
            return SuccessResponse(result, "Shift detail added successfully.");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<CashierShiftDetailDto>(ex.Message, 404); }
        catch (InvalidOperationException ex) { return ErrorResponse<CashierShiftDetailDto>(ex.Message, 400); }
    }

    /// <summary>Delete a shift detail line.</summary>
    [HttpDelete("details/{detailId:guid}")]
    public async Task<ActionResult<ApiResponse>> DeleteDetail(Guid detailId)
    {
        try
        {
            await _mediator.Send(new DeleteCashierShiftDetailCommand(detailId));
            return NoContentResponse();
        }
        catch (KeyNotFoundException ex) { return ErrorResponse(ex.Message, 404); }
    }
}
