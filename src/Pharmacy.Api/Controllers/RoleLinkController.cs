using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.RoleLink;
using Pharmacy.Application.DTOs.RoleLink;
using Pharmacy.Application.Queries.RoleLink;

namespace Pharmacy.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class RoleLinkController : BaseApiController
{
    private readonly IMediator _mediator;

    public RoleLinkController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ── Queries ───────────────────────────────────────────────────────────

    /// <summary>Get a single RoleLink by its ID.</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<RoleLinkDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRoleLinkByIdQuery(id), cancellationToken);
        if (result is null)
            return ErrorResponse<RoleLinkDto>("RoleLink not found", 404);
        return SuccessResponse(result, "RoleLink retrieved successfully");
    }

    /// <summary>Get all permission entries for a specific role.</summary>
    [HttpGet("by-role/{roleId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<RoleLinkDto>>>> GetByRole(Guid roleId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRoleLinksByRoleIdQuery(roleId), cancellationToken);
        return SuccessResponse(result, "Role permissions retrieved successfully");
    }

    /// <summary>Get all roles that have a permission entry for a specific link.</summary>
    [HttpGet("by-link/{linkId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<RoleLinkDto>>>> GetByLink(Guid linkId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRoleLinksByLinkIdQuery(linkId), cancellationToken);
        return SuccessResponse(result, "Link permissions retrieved successfully");
    }

    /// <summary>Get all links accessible (CanRead=true) for a specific role.</summary>
    [HttpGet("accessible/{roleId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<RoleLinkDto>>>> GetAccessibleLinks(Guid roleId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAccessibleLinksForRoleQuery(roleId), cancellationToken);
        return SuccessResponse(result, "Accessible links retrieved successfully");
    }

    // ── Commands ──────────────────────────────────────────────────────────

    /// <summary>Create a single permission entry for a role + link combination.</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<RoleLinkDto>>> Create(
        [FromBody] CreateRoleLinkDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateRoleLinkCommand(dto), cancellationToken);
            return CreatedResponse(result, nameof(GetById), new { id = result.Oid }, "RoleLink created successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<RoleLinkDto>(ex.Message, 409);
        }
        catch (Exception ex)
        {
            return ErrorResponse<RoleLinkDto>($"Error creating RoleLink: {ex.Message}", 500);
        }
    }

    /// <summary>Update the permission flags (CanRead/Write/Edit/Delete) of an existing RoleLink.</summary>
    [HttpPut]
    public async Task<ActionResult<ApiResponse<RoleLinkDto>>> Update(
        [FromBody] UpdateRoleLinkDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new UpdateRoleLinkCommand(dto), cancellationToken);
            return SuccessResponse(result, "RoleLink updated successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<RoleLinkDto>(ex.Message, 404);
        }
        catch (Exception ex)
        {
            return ErrorResponse<RoleLinkDto>($"Error updating RoleLink: {ex.Message}", 500);
        }
    }

    /// <summary>Delete a single permission entry by ID.</summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeleteRoleLinkCommand(id), cancellationToken);
            return SuccessResponse(result, "RoleLink deleted successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<bool>(ex.Message, 404);
        }
        catch (Exception ex)
        {
            return ErrorResponse<bool>($"Error deleting RoleLink: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Replace ALL permission entries for a role in one atomic call.
    /// Existing entries are removed and replaced with the provided list.
    /// </summary>
    [HttpPost("set")]
    public async Task<ActionResult<ApiResponse<IEnumerable<RoleLinkDto>>>> SetRoleLinks(
        [FromBody] SetRoleLinksDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new SetRoleLinksCommand(dto), cancellationToken);
            return SuccessResponse(result, "Role permissions updated successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<IEnumerable<RoleLinkDto>>($"Error setting role permissions: {ex.Message}", 500);
        }
    }
}
