using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.GenericName;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.GenericName;
using Pharmacy.Application.Queries.GenericName;

namespace Pharmacy.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class GenericNameController : BaseApiController
{
    private readonly IMediator _mediator;

    public GenericNameController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get generic name data with advanced filtering, sorting, and pagination.</summary>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<GenericNameDto>>>> GetGenericNameData([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetGenericNameDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Generic name data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<GenericNameDto>>($"Error retrieving generic name data: {ex.Message}", 500);
        }
    }

    /// <summary>Get all generic names.</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<GenericNameDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllGenericNamesQuery(), cancellationToken);
        return SuccessResponse(result, "Generic names retrieved successfully");
    }

    /// <summary>Get a generic name by ID.</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GenericNameDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetGenericNameByIdQuery(id), cancellationToken);

        if (result is null)
            return ErrorResponse<GenericNameDto>("Generic name not found", 404);

        return SuccessResponse(result, "Generic name retrieved successfully");
    }

    /// <summary>Search generic names by English or Arabic term.</summary>
    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<IEnumerable<GenericNameDto>>>> Search([FromQuery] string term, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SearchGenericNamesQuery(term), cancellationToken);
        return SuccessResponse(result, "Search completed successfully");
    }

    /// <summary>Create a new generic name.</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<GenericNameDto>>> Create([FromBody] CreateGenericNameDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateGenericNameCommand(dto), cancellationToken);
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "Generic name created successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<GenericNameDto>(ex.Message, 400);
        }
    }

    /// <summary>Update an existing generic name.</summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<GenericNameDto>>> Update(Guid id, [FromBody] UpdateGenericNameDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (id != dto.Oid)
                return ErrorResponse<GenericNameDto>("ID mismatch", 400);

            var result = await _mediator.Send(new UpdateGenericNameCommand(dto), cancellationToken);
            return SuccessResponse(result, "Generic name updated successfully");
        }
        catch (KeyNotFoundException)
        {
            return ErrorResponse<GenericNameDto>("Generic name not found", 404);
        }
    }

    /// <summary>Delete a generic name (soft delete).</summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _mediator.Send(new DeleteGenericNameCommand(id), cancellationToken);

            if (!deleted)
                return ErrorResponse("Generic name not found", 404);

            return SuccessResponse("Generic name deleted successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse($"Error deleting generic name: {ex.Message}", 500);
        }
    }
}
