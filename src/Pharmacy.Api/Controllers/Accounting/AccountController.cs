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
public class AccountController : BaseApiController
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator) => _mediator = mediator;

    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<AccountDto>>>> Query([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetAccountDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Accounts retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<AccountDto>>($"Error retrieving accounts: {ex.Message}", 500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<AccountDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAccountByIdQuery(id), cancellationToken);
        if (result is null) return ErrorResponse<AccountDto>("Account not found", 404);
        return SuccessResponse(result, "Account retrieved successfully");
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<AccountDto>>> Create([FromBody] CreateAccountDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateAccountCommand(dto), cancellationToken);
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Account created successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<AccountDto>($"Error creating account: {ex.Message}", 500);
        }
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<AccountDto>>> Update([FromBody] UpdateAccountDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new UpdateAccountCommand(dto), cancellationToken);
            return SuccessResponse(result, "Account updated successfully");
        }
        catch (KeyNotFoundException ex) { return ErrorResponse<AccountDto>(ex.Message, 404); }
        catch (Exception ex) { return ErrorResponse<AccountDto>($"Error updating account: {ex.Message}", 500); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeleteAccountCommand(id), cancellationToken);
            if (!result) return ErrorResponse<bool>("Account not found", 404);
            return SuccessResponse(result, "Account deleted successfully");
        }
        catch (Exception ex) { return ErrorResponse<bool>($"Error deleting account: {ex.Message}", 500); }
    }

    /// <summary>
    /// Creates a child account under a parent account and links it to a customer or stakeholder.
    /// Provide either <c>customerId</c> or <c>stakeholderId</c>, not both.
    /// </summary>
    [HttpPost("create-child")]
    public async Task<ActionResult<ApiResponse<AccountDto>>> CreateChildAccount(
        [FromBody] CreateChildAccountDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateChildAccountCommand(dto), cancellationToken);
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Child account created and linked successfully");
        }
        catch (InvalidOperationException ex) { return ErrorResponse<AccountDto>(ex.Message, 400); }
        catch (Exception ex) { return ErrorResponse<AccountDto>($"Error creating child account: {ex.Message}", 500); }
    }
}
