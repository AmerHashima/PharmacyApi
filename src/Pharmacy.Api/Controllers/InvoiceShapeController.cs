using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.InvoiceShape;
using Pharmacy.Application.DTOs.InvoiceShape;

namespace Pharmacy.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class InvoiceShapeController : BaseApiController
{
    private readonly IMediator _mediator;

    public InvoiceShapeController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<ApiResponse<InvoiceShapeDto>>> Create([FromBody] CreateInvoiceShapeDto dto)
    {
        try
        {
            var result = await _mediator.Send(new CreateInvoiceShapeCommand(dto));
            return CreatedResponse(result, nameof(Create), new { id = result.Oid }, "Invoice shape created successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<InvoiceShapeDto>(ex.Message, 404); }
        catch (InvalidOperationException ex) { return ErrorResponse<InvoiceShapeDto>(ex.Message, 400); }
    }
}
