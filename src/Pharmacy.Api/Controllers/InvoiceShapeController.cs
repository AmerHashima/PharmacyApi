using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.InvoiceShape;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.InvoiceShape;
using Pharmacy.Application.Queries.InvoiceShape;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing invoice shapes (print layouts) per branch
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class InvoiceShapeController : BaseApiController
{
    private readonly IMediator _mediator;

    public InvoiceShapeController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Query invoice shapes with advanced filtering, sorting, and pagination
    /// </summary>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<InvoiceShapeDto>>>> GetData([FromBody] QueryRequest request)
    {
        try
        {
            var result = await _mediator.Send(new GetInvoiceShapeDataQuery(request));
            return SuccessResponse(result, "Invoice shape data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<InvoiceShapeDto>>($"Error retrieving invoice shape data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get all invoice shapes for a specific branch
    /// </summary>
    [HttpGet("by-branch/{branchId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<InvoiceShapeDto>>>> GetByBranch(Guid branchId)
    {
        var result = await _mediator.Send(new GetInvoiceShapesByBranchQuery(branchId));
        return SuccessResponse(result, "Invoice shapes retrieved successfully");
    }

    /// <summary>
    /// Get invoice shape by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<InvoiceShapeDto>>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetInvoiceShapeByIdQuery(id));
        if (result == null)
            return ErrorResponse<InvoiceShapeDto>("Invoice shape not found", 404);

        return SuccessResponse(result, "Invoice shape retrieved successfully");
    }

    /// <summary>
    /// Create a new invoice shape
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<InvoiceShapeDto>>> Create([FromBody] CreateInvoiceShapeDto dto)
    {
        try
        {
            var result = await _mediator.Send(new CreateInvoiceShapeCommand(dto));
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Invoice shape created successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<InvoiceShapeDto>(ex.Message, 404); }
        catch (InvalidOperationException ex) { return ErrorResponse<InvoiceShapeDto>(ex.Message, 400); }
    }

    /// <summary>
    /// Update an existing invoice shape
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<InvoiceShapeDto>>> Update(Guid id, [FromBody] UpdateInvoiceShapeDto dto)
    {
        try
        {
            if (id != dto.Oid)
                return ErrorResponse<InvoiceShapeDto>("Invoice shape ID mismatch", 400);

            var result = await _mediator.Send(new UpdateInvoiceShapeCommand(dto));
            return SuccessResponse(result, "Invoice shape updated successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<InvoiceShapeDto>(ex.Message, 404); }
        catch (InvalidOperationException ex) { return ErrorResponse<InvoiceShapeDto>(ex.Message, 400); }
    }

    /// <summary>
    /// Delete an invoice shape (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteInvoiceShapeCommand(id));
        if (!result)
            return ErrorResponse("Invoice shape not found", 404);

        return SuccessResponse("Invoice shape deleted successfully");
    }
}
