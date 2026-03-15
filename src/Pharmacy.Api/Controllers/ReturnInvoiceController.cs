using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.ReturnInvoice;
using Pharmacy.Application.DTOs.ReturnInvoice;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.ReturnInvoice;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing return invoices (refunds/returns)
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class ReturnInvoiceController : BaseApiController
{
    private readonly IMediator _mediator;

    public ReturnInvoiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get return invoice data with advanced filtering, sorting, and pagination
    /// </summary>
    /// <param name="request">Query request with filters, sorting, and pagination</param>
    /// <returns>Paginated return invoice data</returns>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<ReturnInvoiceDto>>>> GetReturnInvoiceData([FromBody] QueryRequest request)
    {
        try
        {
            var query = new GetReturnInvoiceDataQuery(request);
            var result = await _mediator.Send(query);
            return SuccessResponse(result, "Return invoice data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<ReturnInvoiceDto>>($"Error retrieving return invoice data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get return invoices with optional filters
    /// </summary>
    /// <param name="branchId">Optional: Filter by branch</param>
    /// <param name="cashierId">Optional: Filter by cashier</param>
    /// <param name="startDate">Optional: Start date filter</param>
    /// <param name="endDate">Optional: End date filter</param>
    /// <returns>List of return invoices</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ReturnInvoiceDto>>>> GetReturnInvoices(
        [FromQuery] Guid? branchId = null,
        [FromQuery] Guid? cashierId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var query = new GetReturnInvoiceListQuery(branchId, startDate, endDate, cashierId);
        var invoices = await _mediator.Send(query);
        return SuccessResponse(invoices, "Return invoices retrieved successfully");
    }

    /// <summary>
    /// Get return invoice by ID with items
    /// </summary>
    /// <param name="id">Return Invoice ID</param>
    /// <returns>Return invoice details with items</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ReturnInvoiceDto>>> GetReturnInvoice(Guid id)
    {
        var query = new GetReturnInvoiceByIdQuery(id);
        var invoice = await _mediator.Send(query);

        if (invoice == null)
            return ErrorResponse<ReturnInvoiceDto>("Return invoice not found", 404);

        return SuccessResponse(invoice, "Return invoice retrieved successfully");
    }

    /// <summary>
    /// Create a new return invoice (refund/return transaction)
    /// This will also create stock IN transactions for each returned item
    /// </summary>
    /// <param name="createReturnInvoiceDto">Return invoice creation data with items</param>
    /// <returns>Created return invoice</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ReturnInvoiceDto>>> CreateReturnInvoice([FromBody] CreateReturnInvoiceDto createReturnInvoiceDto)
    {
        try
        {
            var command = new CreateReturnInvoiceCommand(createReturnInvoiceDto);
            var invoice = await _mediator.Send(command);
            return CreatedResponse(invoice, nameof(GetReturnInvoice), new { id = invoice.Oid }, "Return invoice created successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<ReturnInvoiceDto>(ex.Message, 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<ReturnInvoiceDto>(ex.Message, 400);
        }
    }
}
