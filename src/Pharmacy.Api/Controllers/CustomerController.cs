using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.Customer;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Customer;
using Pharmacy.Application.Queries.Customer;

namespace Pharmacy.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class CustomerController : BaseApiController
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get customer data with advanced filtering, sorting, and pagination.</summary>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<CustomerDto>>>> GetCustomerData([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetCustomerDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Customer data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<CustomerDto>>($"Error retrieving customer data: {ex.Message}", 500);
        }
    }

    /// <summary>Get a customer by ID.</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustomerByIdQuery(id), cancellationToken);

        if (result is null)
            return ErrorResponse<CustomerDto>("Customer not found", 404);

        return SuccessResponse(result, "Customer retrieved successfully");
    }

    /// <summary>Create a new customer.</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> Create([FromBody] CreateCustomerDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateCustomerCommand(dto), cancellationToken);
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Customer created successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<CustomerDto>(ex.Message, 400);
        }
    }

    /// <summary>Update an existing customer.</summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> Update(Guid id, [FromBody] UpdateCustomerDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (id != dto.Oid)
                return ErrorResponse<CustomerDto>("ID mismatch", 400);

            var result = await _mediator.Send(new UpdateCustomerCommand(dto), cancellationToken);
            return SuccessResponse(result, "Customer updated successfully");
        }
        catch (KeyNotFoundException)
        {
            return ErrorResponse<CustomerDto>("Customer not found", 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<CustomerDto>(ex.Message, 400);
        }
    }

    /// <summary>Delete a customer (soft delete).</summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _mediator.Send(new DeleteCustomerCommand(id), cancellationToken);

            if (!deleted)
                return ErrorResponse("Customer not found", 404);

            return SuccessResponse("Customer deleted successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse(ex.Message, 400);
        }
        catch (Exception ex)
        {
            return ErrorResponse($"Error deleting customer: {ex.Message}", 500);
        }
    }
}
