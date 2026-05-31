using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.PurchaseInvoice;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.PurchaseInvoice;
using Pharmacy.Application.Queries.PurchaseInvoice;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Manages purchase invoices (supplier invoices) and their payments.
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class PurchaseInvoiceController : BaseApiController
{
    private readonly IMediator _mediator;

    public PurchaseInvoiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ─────────────────────────────────────────────────────────────────────
    // GET
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>Get a purchase invoice by ID (includes payments).</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<PurchaseInvoiceDto>>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetPurchaseInvoiceByIdQuery(id));
        if (result == null)
            return ErrorResponse<PurchaseInvoiceDto>("Purchase invoice not found.", 404);
        return SuccessResponse(result);
    }

    /// <summary>Advanced filter / sort / paginate purchase invoices.</summary>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<PurchaseInvoiceDto>>>> Query([FromBody] QueryRequest request)
    {
        try
        {
            var result = await _mediator.Send(new GetPurchaseInvoiceDataQuery(request));
            return SuccessResponse(result);
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<PurchaseInvoiceDto>>(ex.Message, 500);
        }
    }

    /// <summary>Get all purchase invoices for a branch.</summary>
    [HttpGet("branch/{branchId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PurchaseInvoiceDto>>>> GetByBranch(Guid branchId)
    {
        var result = await _mediator.Send(new GetPurchaseInvoicesByBranchQuery(branchId));
        return SuccessResponse(result);
    }

    /// <summary>Get all purchase invoices for a supplier.</summary>
    [HttpGet("supplier/{supplierId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PurchaseInvoiceDto>>>> GetBySupplier(Guid supplierId)
    {
        var result = await _mediator.Send(new GetPurchaseInvoicesBySupplierQuery(supplierId));
        return SuccessResponse(result);
    }

    /// <summary>Get all payments for a purchase invoice.</summary>
    [HttpGet("{id:guid}/payments")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PurchaseInvoicePaymentDto>>>> GetPayments(Guid id)
    {
        var result = await _mediator.Send(new GetPurchaseInvoicePaymentsQuery(id));
        return SuccessResponse(result);
    }

    // ─────────────────────────────────────────────────────────────────────
    // POST / PUT / DELETE
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>Create a new purchase invoice (optionally with initial payments).</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PurchaseInvoiceDto>>> Create([FromBody] CreatePurchaseInvoiceDto dto)
    {
        try
        {
            var result = await _mediator.Send(new CreatePurchaseInvoiceCommand(dto));
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Purchase invoice created successfully.");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<PurchaseInvoiceDto>(ex.Message, 404); }
        catch (InvalidOperationException ex) { return ErrorResponse<PurchaseInvoiceDto>(ex.Message, 400); }
    }

    /// <summary>Update a purchase invoice header.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<PurchaseInvoiceDto>>> Update(Guid id, [FromBody] UpdatePurchaseInvoiceDto dto)
    {
        try
        {
            var result = await _mediator.Send(new UpdatePurchaseInvoiceCommand(id, dto));
            return SuccessResponse(result, "Purchase invoice updated successfully.");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<PurchaseInvoiceDto>(ex.Message, 404); }
    }

    /// <summary>Soft-delete a purchase invoice.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id)
    {
        try
        {
            await _mediator.Send(new DeletePurchaseInvoiceCommand(id));
            return NoContentResponse();
        }
        catch (KeyNotFoundException ex) { return ErrorResponse(ex.Message, 404); }
    }

    // ─────────────────────────────────────────────────────────────────────
    // Payments
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>Add a payment to an existing purchase invoice.</summary>
    [HttpPost("{id:guid}/payments")]
    public async Task<ActionResult<ApiResponse<PurchaseInvoicePaymentDto>>> AddPayment(Guid id, [FromBody] CreatePurchaseInvoicePaymentDto dto)
    {
        try
        {
            var result = await _mediator.Send(new AddPurchaseInvoicePaymentCommand(id, dto));
            return SuccessResponse(result, "Payment added successfully.");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<PurchaseInvoicePaymentDto>(ex.Message, 404); }
        catch (InvalidOperationException ex) { return ErrorResponse<PurchaseInvoicePaymentDto>(ex.Message, 400); }
    }

    /// <summary>Delete a payment from a purchase invoice.</summary>
    [HttpDelete("payments/{paymentId:guid}")]
    public async Task<ActionResult<ApiResponse>> DeletePayment(Guid paymentId)
    {
        try
        {
            await _mediator.Send(new DeletePurchaseInvoicePaymentCommand(paymentId));
            return NoContentResponse();
        }
        catch (KeyNotFoundException ex) { return ErrorResponse(ex.Message, 404); }
    }
}
