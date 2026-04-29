using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.Link;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Link;
using Pharmacy.Application.Queries.Link;

namespace Pharmacy.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class LinkController : BaseApiController
{
    private readonly IMediator _mediator;

    public LinkController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all active links (navigation items and reports) for the UI menu.</summary>
    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<IEnumerable<LinkDto>>>> GetActiveLinks(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetActiveLinksQuery(), cancellationToken);
            return SuccessResponse(result, "Active links retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<IEnumerable<LinkDto>>($"Error retrieving active links: {ex.Message}", 500);
        }
    }

    /// <summary>Get link data with advanced filtering, sorting, and pagination.</summary>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<LinkDto>>>> GetLinkData(
        [FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetLinkDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Link data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<LinkDto>>($"Error retrieving link data: {ex.Message}", 500);
        }
    }

    /// <summary>Get a link by ID including its report parameters.</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<LinkDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLinkByIdQuery(id), cancellationToken);

        if (result is null)
            return ErrorResponse<LinkDto>("Link not found", 404);

        return SuccessResponse(result, "Link retrieved successfully");
    }

    /// <summary>Create a new link with optional report parameters.</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<LinkDto>>> Create(
        [FromBody] CreateLinkDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateLinkCommand(dto), cancellationToken);
            return CreatedResponse(result, "Link created successfully", new { id = result.Oid });
        }
        catch (Exception ex)
        {
            return ErrorResponse<LinkDto>($"Error creating link: {ex.Message}", 500);
        }
    }

    /// <summary>Update an existing link and replace its report parameters.</summary>
    [HttpPut]
    public async Task<ActionResult<ApiResponse<LinkDto>>> Update(
        [FromBody] UpdateLinkDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new UpdateLinkCommand(dto), cancellationToken);
            return SuccessResponse(result, "Link updated successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<LinkDto>(ex.Message, 404);
        }
        catch (Exception ex)
        {
            return ErrorResponse<LinkDto>($"Error updating link: {ex.Message}", 500);
        }
    }

    /// <summary>Soft-delete a link and all its report parameters.</summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeleteLinkCommand(id), cancellationToken);

            if (!result)
                return ErrorResponse<bool>("Link not found", 404);

            return SuccessResponse(result, "Link deleted successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<bool>($"Error deleting link: {ex.Message}", 500);
        }
    }
}
