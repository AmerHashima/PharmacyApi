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
public class PaymentVoucherController : BaseApiController
{
    private readonly IMediator _mediator;

    public PaymentVoucherController(IMediator mediator) => _mediator = mediator;   

    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<PaymentVoucherDto>>>> Query([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetPaymentVoucherDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Payment vouchers retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<PaymentVoucherDto>>($"Error retrieving payment vouchers: {ex.Message}", 500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PaymentVoucherDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPaymentVoucherByIdQuery(id), cancellationToken);
        if (result is null) return ErrorResponse<PaymentVoucherDto>("Payment voucher not found", 404);
        return SuccessResponse(result, "Payment voucher retrieved successfully");
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PaymentVoucherDto>>> Create([FromBody] CreatePaymentVoucherDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreatePaymentVoucherCommand(dto), cancellationToken);
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Payment voucher created successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PaymentVoucherDto>($"Error creating payment voucher: {ex.Message}", 500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<PaymentVoucherDto>>> Update([FromBody] UpdatePaymentVoucherDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new UpdatePaymentVoucherCommand(dto), cancellationToken);
            return SuccessResponse(result, "Payment voucher updated successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<PaymentVoucherDto>(ex.Message, 404); }
        catch (Exception ex) { return ErrorResponse<PaymentVoucherDto>($"Error updating payment voucher: {ex.Message}", 500); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeletePaymentVoucherCommand(id), cancellationToken);
            if (!result) return ErrorResponse<bool>("Payment voucher not found", 404);
            return SuccessResponse(result, "Payment voucher deleted successfully");
        }
        catch (Exception ex) { return ErrorResponse<bool>($"Error deleting payment voucher: {ex.Message}", 500); }
    }
}
