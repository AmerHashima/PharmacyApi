using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.SystemUserSpace;
using Pharmacy.Application.DTOs.SystemUserSpace;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.SystemUserSpace;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class SystemUserController : BaseApiController
{
    private readonly IMediator _mediator;

    public SystemUserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get system user data with advanced filtering, sorting, and pagination
    /// </summary>
    /// <param name="request">Query request with filters, sorting, and pagination</param>
    /// <returns>Paginated system user data</returns>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<SystemUserDto>>>> GetSystemUserData([FromBody] QueryRequest request)
    {
        try
        {
            var query = new GetSystemUserDataQuery(request);
            var result = await _mediator.Send(query);
            return SuccessResponse(result, "System user data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<SystemUserDto>>($"Error retrieving system user data: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Get all system users with optional filtering
    /// </summary>
    /// <param name="includeInactive">Include inactive users</param>
    /// <param name="roleId">Filter by role ID</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>List of system users</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<SystemUserDto>>>> GetSystemUsers(
        [FromQuery] bool includeInactive = false,
        [FromQuery] int? roleId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = new GetSystemUserListQuery(includeInactive, roleId);
        var users = await _mediator.Send(query);
        return SuccessResponse(users, "System users retrieved successfully");
    }

    /// <summary>
    /// Get system user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<SystemUserDto>>> GetSystemUser(Guid id)
    {
        var query = new GetSystemUserByIdQuery(id);
        var user = await _mediator.Send(query);

        if (user == null)
            return ErrorResponse<SystemUserDto>("User not found", 404);

        return SuccessResponse(user, "User retrieved successfully");
    }

    /// <summary>
    /// Create a new system user
    /// </summary>
    /// <param name="createUserDto">User creation data</param>
    /// <returns>Created user</returns>
    [HttpPost]
    //[Authorize(Roles = "Admin")] // Only admins can create users
    public async Task<ActionResult<ApiResponse<SystemUserDto>>> CreateSystemUser([FromBody] CreateSystemUserDto createUserDto)
    {
        try
        {
            var command = new CreateSystemUserCommand(createUserDto);
            var user = await _mediator.Send(command);
            return CreatedResponse(user, nameof(GetSystemUser), new { id = user.Oid }, "System user created successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<SystemUserDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Update an existing system user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="updateUserDto">User update data</param>
    /// <returns>Updated user</returns>
    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin")] // Only admins can update users
    public async Task<ActionResult<ApiResponse<SystemUserDto>>> UpdateSystemUser(Guid id, [FromBody] UpdateSystemUserDto updateUserDto)
    {
        try
        {
            if (id != updateUserDto.Oid)
                return ErrorResponse<SystemUserDto>("User ID mismatch", 400);

            var command = new UpdateSystemUserCommand(updateUserDto);
            var user = await _mediator.Send(command);
            return SuccessResponse(user, "System user updated successfully");
        }
        catch (KeyNotFoundException)
        {
            return ErrorResponse<SystemUserDto>("User not found", 404);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<SystemUserDto>(ex.Message, 400);
        }
    }

    /// <summary>
    /// Delete a system user (soft delete)
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>Success response</returns>
    [HttpDelete("{id}")]
   // [Authorize(Roles = "Admin")] // Only admins can delete users
    public async Task<ActionResult<ApiResponse>> DeleteSystemUser(Guid id)
    {
        try
        {
            var command = new DeleteSystemUserCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
                return ErrorResponse("User not found", 404);

            return SuccessResponse("System user deleted successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse($"Error deleting user: {ex.Message}", 500);
        }
    }
}