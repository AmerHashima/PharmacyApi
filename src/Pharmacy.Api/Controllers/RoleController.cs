using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.Role;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Role;
using Pharmacy.Application.Queries.Role;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Api.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class RoleController : BaseApiController
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get role data with advanced filtering, sorting, and pagination
    /// </summary>
    /// <param name="request">Query request with filters, sorting, and pagination</param>
    /// <returns>Paginated role data</returns>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<RoleDto>>>> GetRoleData([FromBody] QueryRequest request)
    {
        try
        {
            var query = new GetRoleDataQuery(request);
            var result = await _mediator.Send(query);
            return SuccessResponse(result, "Role data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<RoleDto>>($"Error retrieving role data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns>List of roles</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> GetRoles()
    {
        var query = new GetRoleListQuery();
        var roles = await _mediator.Send(query);
        return SuccessResponse(roles, "Roles retrieved successfully");
    }

    /// <summary>
    /// Get role by ID
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <returns>Role details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<RoleDto>>> GetRole(Guid id)
    {
        var query = new GetRoleByIdQuery(id);
        var role = await _mediator.Send(query);

        if (role == null)
            return ErrorResponse<RoleDto>("Role not found", 404);

        return SuccessResponse(role, "Role retrieved successfully");
    }

    /// <summary>
    /// Create a new role
    /// </summary>
    /// <param name="createRoleDto">Role creation data</param>
    /// <returns>Created role</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<RoleDto>>> CreateRole([FromBody] CreateRoleDto createRoleDto)
    {
        try
        {
            var command = new CreateRoleCommand(createRoleDto);
            var role = await _mediator.Send(command);
            return CreatedResponse(role, nameof(GetRole), new { id = role.Oid }, "Role created successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<RoleDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Update an existing role
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <param name="updateRoleDto">Role update data</param>
    /// <returns>Updated role</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<RoleDto>>> UpdateRole(Guid id, [FromBody] UpdateRoleDto updateRoleDto)
    {
        try
        {
            if (id != updateRoleDto.Oid)
                return ErrorResponse<RoleDto>("Role ID mismatch", 400);

            var command = new UpdateRoleCommand(updateRoleDto);
            var role = await _mediator.Send(command);
            return SuccessResponse(role, "Role updated successfully");
        }
        catch (KeyNotFoundException)
        {
            return ErrorResponse<RoleDto>("Role not found", 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<RoleDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Delete a role (soft delete)
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <returns>Success response</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteRole(Guid id)
    {
        try
        {
            var command = new DeleteRoleCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
                return ErrorResponse("Role not found", 404);

            return SuccessResponse("Role deleted successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse($"Error deleting role: {ex.Message}", 500);
        }
    }
}