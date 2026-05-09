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
public class BankAccountController : BaseApiController
{
    private readonly IMediator _mediator;

    public BankAccountController(IMediator mediator) => _mediator = mediator;

    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<BankAccountDto>>>> Query([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetBankAccountDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Bank accounts retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<BankAccountDto>>($"Error retrieving bank accounts: {ex.Message}", 500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BankAccountDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBankAccountByIdQuery(id), cancellationToken);
        if (result is null) return ErrorResponse<BankAccountDto>("Bank account not found", 404);
        return SuccessResponse(result, "Bank account retrieved successfully");
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BankAccountDto>>> Create([FromBody] CreateBankAccountDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateBankAccountCommand(dto), cancellationToken);
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Bank account created successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<BankAccountDto>($"Error creating bank account: {ex.Message}", 500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<BankAccountDto>>> Update([FromBody] UpdateBankAccountDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new UpdateBankAccountCommand(dto), cancellationToken);
            return SuccessResponse(result, "Bank account updated successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<BankAccountDto>(ex.Message, 404); }
        catch (Exception ex) { return ErrorResponse<BankAccountDto>($"Error updating bank account: {ex.Message}", 500); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeleteBankAccountCommand(id), cancellationToken);
            if (!result) return ErrorResponse<bool>("Bank account not found", 404);
            return SuccessResponse(result, "Bank account deleted successfully");
        }
        catch (Exception ex) { return ErrorResponse<bool>($"Error deleting bank account: {ex.Message}", 500); }
    }
}
