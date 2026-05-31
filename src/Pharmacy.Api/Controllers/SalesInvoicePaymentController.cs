using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.SalesInvoicePayment;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.SalesInvoicePayment;
using Pharmacy.Application.Queries.SalesInvoicePayment;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Manages individual payment records linked to sales invoices.
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class SalesInvoicePaymentController : BaseApiController
{
    private readonly IMediator _mediator;

    public SalesInvoicePaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ─────────────────────────────────────────────────────────────────────
    // Queries
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>Advanced filter / sort / paginate sales invoice payments.</summary>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<SalesInvoicePaymentDto>>>> Query([FromBody] QueryRequest request)
    {
        try
        {
            var result = await _mediator.Send(new GetSalesInvoicePaymentDataQuery(request));
            return SuccessResponse(result);
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<SalesInvoicePaymentDto>>(ex.Message, 500);
        }
    }

    /// <summary>Get all payments for a specific sales invoice.</summary>
    [HttpGet("invoice/{salesInvoiceId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<SalesInvoicePaymentDto>>>> GetByInvoice(Guid salesInvoiceId)
    {
        var result = await _mediator.Send(new GetSalesInvoicePaymentsByInvoiceQuery(salesInvoiceId));
        return SuccessResponse(result);
    }

    /// <summary>Get all payments recorded under a cashier shift.</summary>
    [HttpGet("shift/{shiftId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<SalesInvoicePaymentDto>>>> GetByShift(Guid shiftId)
    {
        var result = await _mediator.Send(new GetSalesInvoicePaymentsByShiftQuery(shiftId));
        return SuccessResponse(result);
    }

    // ─────────────────────────────────────────────────────────────────────
    // Commands
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>Record a new payment against a sales invoice.</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalesInvoicePaymentDto>>> Create([FromBody] CreateSalesInvoicePaymentDto dto)
    {
        try
        {
            var result = await _mediator.Send(new CreateSalesInvoicePaymentCommand(dto));
            return SuccessResponse(result, "Sales invoice payment recorded successfully.");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<SalesInvoicePaymentDto>(ex.Message, 404); }
        catch (InvalidOperationException ex) { return ErrorResponse<SalesInvoicePaymentDto>(ex.Message, 400); }
    }

    /// <summary>Delete a sales invoice payment record.</summary>
    [HttpDelete("{paymentId:guid}")]
    public async Task<ActionResult<ApiResponse>> Delete(Guid paymentId)
    {
        try
        {
            await _mediator.Send(new DeleteSalesInvoicePaymentCommand(paymentId));
            return NoContentResponse();
        }
        catch (KeyNotFoundException ex) { return ErrorResponse(ex.Message, 404); }
    }
}
