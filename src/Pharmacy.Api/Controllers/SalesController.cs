using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.SalesInvoice;
using Pharmacy.Application.DTOs.SalesInvoice;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.SalesInvoice;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing sales invoices (POS transactions)
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class SalesController : BaseApiController
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get sales invoice data with advanced filtering, sorting, and pagination
    /// </summary>
    /// <param name="request">Query request with filters, sorting, and pagination</param>
    /// <returns>Paginated sales invoice data</returns>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<SalesInvoiceDto>>>> GetSalesData([FromBody] QueryRequest request)
    {
        try
        {
            var query = new GetSalesInvoiceDataQuery(request);
            var result = await _mediator.Send(query);
            return SuccessResponse(result, "Sales data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<SalesInvoiceDto>>($"Error retrieving sales data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get sales invoices with optional filters
    /// </summary>
    /// <param name="branchId">Optional: Filter by branch</param>
    /// <param name="cashierId">Optional: Filter by cashier</param>
    /// <param name="startDate">Optional: Start date filter</param>
    /// <param name="endDate">Optional: End date filter</param>
    /// <returns>List of sales invoices</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<SalesInvoiceDto>>>> GetSalesInvoices(
        [FromQuery] Guid? branchId = null,
        [FromQuery] Guid? cashierId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var query = new GetSalesInvoiceListQuery(branchId, startDate, endDate, cashierId);
        var invoices = await _mediator.Send(query);
        return SuccessResponse(invoices, "Sales invoices retrieved successfully");
    }

    /// <summary>
    /// Get sales invoice by ID with items
    /// </summary>
    /// <param name="id">Invoice ID</param>
    /// <returns>Invoice details with items</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<SalesInvoiceDto>>> GetSalesInvoice(Guid id)
    {
        var query = new GetSalesInvoiceByIdQuery(id);
        var invoice = await _mediator.Send(query);

        if (invoice == null)
            return ErrorResponse<SalesInvoiceDto>("Sales invoice not found", 404);

        return SuccessResponse(invoice, "Sales invoice retrieved successfully");
    }

    /// <summary>
    /// Create a new sales invoice (POS transaction)
    /// This will also create stock OUT transactions for each item
    /// </summary>
    /// <param name="createInvoiceDto">Invoice creation data with items</param>
    /// <returns>Created invoice</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalesInvoiceDto>>> CreateSalesInvoice([FromBody] CreateSalesInvoiceDto createInvoiceDto)
    {
        try
        {
            var command = new CreateSalesInvoiceCommand(createInvoiceDto);
            var invoice = await _mediator.Send(command);
            return CreatedResponse(invoice, nameof(GetSalesInvoice), new { id = invoice.Oid }, "Sales invoice created successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<SalesInvoiceDto>(ex.Message, 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<SalesInvoiceDto>(ex.Message, 400);
        }
    }
}
