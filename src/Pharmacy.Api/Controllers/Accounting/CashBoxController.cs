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
public class CashBoxController : BaseApiController
{
    private readonly IMediator _mediator;

    public CashBoxController(IMediator mediator) => _mediator = mediator;

    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<CashBoxDto>>>> Query([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetCashBoxDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Cash boxes retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<CashBoxDto>>($"Error retrieving cash boxes: {ex.Message}", 500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CashBoxDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCashBoxByIdQuery(id), cancellationToken);
        if (result is null) return ErrorResponse<CashBoxDto>("Cash box not found", 404);
        return SuccessResponse(result, "Cash box retrieved successfully");
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CashBoxDto>>> Create([FromBody] CreateCashBoxDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateCashBoxCommand(dto), cancellationToken);
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Cash box created successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<CashBoxDto>($"Error creating cash box: {ex.Message}", 500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<CashBoxDto>>> Update([FromBody] UpdateCashBoxDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new UpdateCashBoxCommand(dto), cancellationToken);
            return SuccessResponse(result, "Cash box updated successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<CashBoxDto>(ex.Message, 404); }
        catch (Exception ex) { return ErrorResponse<CashBoxDto>($"Error updating cash box: {ex.Message}", 500); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeleteCashBoxCommand(id), cancellationToken);
            if (!result) return ErrorResponse<bool>("Cash box not found", 404);
            return SuccessResponse(result, "Cash box deleted successfully");
        }
        catch (Exception ex) { return ErrorResponse<bool>($"Error deleting cash box: {ex.Message}", 500); }
    }
}
