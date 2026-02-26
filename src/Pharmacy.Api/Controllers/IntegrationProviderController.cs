using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.IntegrationProvider;
using Pharmacy.Application.DTOs.IntegrationProvider;
using Pharmacy.Application.Queries.IntegrationProvider;

namespace Pharmacy.Api.Controllers;

/// <summary>
/// Controller for managing integration providers
/// </summary>
[Route("api/[controller]")]
[Authorize]
public class IntegrationProviderController : BaseApiController
{
    private readonly IMediator _mediator;

    public IntegrationProviderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all integration providers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<IntegrationProviderDto>>>> GetIntegrationProviders()
    {
        var query = new GetIntegrationProviderListQuery();
        var providers = await _mediator.Send(query);
        return SuccessResponse(providers, "Integration providers retrieved successfully");
    }

    /// <summary>
    /// Get integration provider by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<IntegrationProviderDto>>> GetIntegrationProvider(Guid id)
    {
        var query = new GetIntegrationProviderByIdQuery(id);
        var provider = await _mediator.Send(query);

        if (provider == null)
            return ErrorResponse<IntegrationProviderDto>("Integration provider not found", 404);

        return SuccessResponse(provider, "Integration provider retrieved successfully");
    }

    /// <summary>
    /// Create a new integration provider
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<IntegrationProviderDto>>> CreateIntegrationProvider([FromBody] CreateIntegrationProviderDto dto)
    {
        try
        {
            var command = new CreateIntegrationProviderCommand(dto);
            var provider = await _mediator.Send(command);
            return CreatedResponse(provider, nameof(GetIntegrationProvider), new { id = provider.Oid }, "Integration provider created successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<IntegrationProviderDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Update an existing integration provider
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<IntegrationProviderDto>>> UpdateIntegrationProvider(Guid id, [FromBody] UpdateIntegrationProviderDto dto)
    {
        if (id != dto.Oid)
            return ErrorResponse<IntegrationProviderDto>("ID mismatch", 400);

        try
        {
            var command = new UpdateIntegrationProviderCommand(dto);
            var provider = await _mediator.Send(command);
            return SuccessResponse(provider, "Integration provider updated successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<IntegrationProviderDto>(ex.Message, 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<IntegrationProviderDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Delete an integration provider
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteIntegrationProvider(Guid id)
    {
        try
        {
            var command = new DeleteIntegrationProviderCommand(id);
            var result = await _mediator.Send(command);
            return SuccessResponse(result, "Integration provider deleted successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<bool>(ex.Message, 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<bool>(ex.Message, 400);
        }
    }
}