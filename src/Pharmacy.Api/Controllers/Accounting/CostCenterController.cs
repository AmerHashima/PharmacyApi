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
public class CostCenterController : BaseApiController
{
    private readonly IMediator _mediator;

    public CostCenterController(IMediator mediator) => _mediator = mediator;

    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<CostCenterDto>>>> Query([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetCostCenterDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Cost centers retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<CostCenterDto>>($"Error retrieving cost centers: {ex.Message}", 500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CostCenterDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCostCenterByIdQuery(id), cancellationToken);
        if (result is null) return ErrorResponse<CostCenterDto>("Cost center not found", 404);
        return SuccessResponse(result, "Cost center retrieved successfully");
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CostCenterDto>>> Create([FromBody] CreateCostCenterDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateCostCenterCommand(dto), cancellationToken);
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Cost center created successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<CostCenterDto>($"Error creating cost center: {ex.Message}", 500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<CostCenterDto>>> Update([FromBody] UpdateCostCenterDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new UpdateCostCenterCommand(dto), cancellationToken);
            return SuccessResponse(result, "Cost center updated successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<CostCenterDto>(ex.Message, 404); }
        catch (Exception ex) { return ErrorResponse<CostCenterDto>($"Error updating cost center: {ex.Message}", 500); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeleteCostCenterCommand(id), cancellationToken);
            if (!result) return ErrorResponse<bool>("Cost center not found", 404);
            return SuccessResponse(result, "Cost center deleted successfully");
        }
        catch (Exception ex) { return ErrorResponse<bool>($"Error deleting cost center: {ex.Message}", 500); }
    }
}
