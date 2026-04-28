using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.Offer;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Offer;
using Pharmacy.Application.Queries.Offer;

namespace Pharmacy.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class OfferController : BaseApiController
{
    private readonly IMediator _mediator;

    public OfferController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get offer data with advanced filtering, sorting, and pagination.</summary>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<OfferMasterDto>>>> GetOfferData(
        [FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetOfferMasterDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Offer data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<OfferMasterDto>>($"Error retrieving offer data: {ex.Message}", 500);
        }
    }

    /// <summary>Get all currently active offers (within valid date range).</summary>
    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<IEnumerable<OfferMasterDto>>>> GetActiveOffers(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetActiveOffersQuery(), cancellationToken);
            return SuccessResponse(result, "Active offers retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<IEnumerable<OfferMasterDto>>($"Error retrieving active offers: {ex.Message}", 500);
        }
    }

    /// <summary>Get an offer by ID including all detail lines.</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<OfferMasterDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOfferMasterByIdQuery(id), cancellationToken);

        if (result is null)
            return ErrorResponse<OfferMasterDto>("Offer not found", 404);

        return SuccessResponse(result, "Offer retrieved successfully");
    }

    /// <summary>Create a new offer with its detail lines.</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<OfferMasterDto>>> Create(
        [FromBody] CreateOfferMasterDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateOfferMasterCommand(dto), cancellationToken);
            return CreatedResponse(result, "Offer created successfully", new { id = result.Oid });
        }
        catch (Exception ex)
        {
            return ErrorResponse<OfferMasterDto>($"Error creating offer: {ex.Message}", 500);
        }
    }

    /// <summary>Update an existing offer and replace its detail lines.</summary>
    [HttpPut]
    public async Task<ActionResult<ApiResponse<OfferMasterDto>>> Update(
        [FromBody] UpdateOfferMasterDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new UpdateOfferMasterCommand(dto), cancellationToken);
            return SuccessResponse(result, "Offer updated successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<OfferMasterDto>(ex.Message, 404);
        }
        catch (Exception ex)
        {
            return ErrorResponse<OfferMasterDto>($"Error updating offer: {ex.Message}", 500);
        }
    }

    /// <summary>Delete an offer and all its detail lines.</summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeleteOfferMasterCommand(id), cancellationToken);

            if (!result)
                return ErrorResponse<bool>("Offer not found", 404);

            return SuccessResponse(result, "Offer deleted successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<bool>($"Error deleting offer: {ex.Message}", 500);
        }
    }
}
