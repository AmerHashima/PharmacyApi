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
public class FiscalYearController : BaseApiController
{
    private readonly IMediator _mediator;

    public FiscalYearController(IMediator mediator) => _mediator = mediator;

    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<FiscalYearDto>>>> Query([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetFiscalYearDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Fiscal years retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<FiscalYearDto>>($"Error retrieving fiscal years: {ex.Message}", 500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<FiscalYearDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFiscalYearByIdQuery(id), cancellationToken);
        if (result is null) return ErrorResponse<FiscalYearDto>("Fiscal year not found", 404);
        return SuccessResponse(result, "Fiscal year retrieved successfully");
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<FiscalYearDto>>> Create([FromBody] CreateFiscalYearDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateFiscalYearCommand(dto), cancellationToken);
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Fiscal year created successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<FiscalYearDto>($"Error creating fiscal year: {ex.Message}", 500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<FiscalYearDto>>> Update([FromBody] UpdateFiscalYearDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new UpdateFiscalYearCommand(dto), cancellationToken);
            return SuccessResponse(result, "Fiscal year updated successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<FiscalYearDto>(ex.Message, 404); }
        catch (Exception ex) { return ErrorResponse<FiscalYearDto>($"Error updating fiscal year: {ex.Message}", 500); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeleteFiscalYearCommand(id), cancellationToken);
            if (!result) return ErrorResponse<bool>("Fiscal year not found", 404);
            return SuccessResponse(result, "Fiscal year deleted successfully");
        }
        catch (Exception ex) { return ErrorResponse<bool>($"Error deleting fiscal year: {ex.Message}", 500); }
    }
}
