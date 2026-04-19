using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.InvoiceSetup;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.InvoiceSetup;
using Pharmacy.Application.Queries.InvoiceSetup;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Manages invoice type setups (e.g. POS Invoice, Supplier Invoice) per branch
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class InvoiceSetupController : BaseApiController
{
    private readonly IMediator _mediator;

    public InvoiceSetupController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get a single invoice setup by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<InvoiceSetupDto>>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetInvoiceSetupByIdQuery(id));
        return result is null
            ? ErrorResponse<InvoiceSetupDto>("Invoice setup not found", 404)
            : SuccessResponse(result);
    }

    /// <summary>Get all invoice setups for a specific branch</summary>
    [HttpGet("branch/{branchId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<InvoiceSetupDto>>>> GetByBranch(Guid branchId)
    {
        var result = await _mediator.Send(new GetInvoiceSetupsByBranchQuery(branchId));
        return SuccessResponse(result, "Invoice setups retrieved successfully");
    }

    /// <summary>Get global (non-branch-specific) invoice setup templates</summary>
    [HttpGet("global")]
    public async Task<ActionResult<ApiResponse<IEnumerable<InvoiceSetupDto>>>> GetGlobal()
    {
        var result = await _mediator.Send(new GetGlobalInvoiceSetupsQuery());
        return SuccessResponse(result, "Global invoice setups retrieved successfully");
    }

    /// <summary>Query invoice setups with filtering, sorting, and pagination</summary>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<InvoiceSetupDto>>>> Query([FromBody] QueryRequest request)
    {
        var result = await _mediator.Send(new GetInvoiceSetupDataQuery(request));
        return SuccessResponse(result, "Invoice setups retrieved successfully");
    }

    /// <summary>Create a new invoice setup</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<InvoiceSetupDto>>> Create([FromBody] CreateInvoiceSetupDto dto)
    {
        try
        {
            var result = await _mediator.Send(new CreateInvoiceSetupCommand(dto));
            return SuccessResponse(result, "Invoice setup created successfully");
        }
        catch (InvalidOperationException ex) { return ErrorResponse<InvoiceSetupDto>(ex.Message, 400); }
    }

    /// <summary>Update an existing invoice setup</summary>
    [HttpPut]
    public async Task<ActionResult<ApiResponse<InvoiceSetupDto>>> Update([FromBody] UpdateInvoiceSetupDto dto)
    {
        try
        {
            var result = await _mediator.Send(new UpdateInvoiceSetupCommand(dto));
            return SuccessResponse(result, "Invoice setup updated successfully");
        }
        catch (InvalidOperationException ex) { return ErrorResponse<InvoiceSetupDto>(ex.Message, 400); }
    }

    /// <summary>Delete an invoice setup</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
    {
        try
        {
            await _mediator.Send(new DeleteInvoiceSetupCommand(id));
            return SuccessResponse(true, "Invoice setup deleted successfully");
        }
        catch (InvalidOperationException ex) { return ErrorResponse<bool>(ex.Message, 400); }
    }
}
