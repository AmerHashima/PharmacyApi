using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Models;
using Pharmacy.Application.Commands.Doctor;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Doctor;
using Pharmacy.Application.Queries.Doctor;

namespace Pharmacy.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
public class DoctorController : BaseApiController
{
    private readonly IMediator _mediator;

    public DoctorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get doctor data with advanced filtering, sorting, and pagination.</summary>
    [HttpPost("query")]
    public async Task<ActionResult<ApiResponse<PagedResult<DoctorDto>>>> GetDoctorData([FromBody] QueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetDoctorDataQuery(request), cancellationToken);
            return SuccessResponse(result, "Doctor data retrieved successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<PagedResult<DoctorDto>>($"Error retrieving doctor data: {ex.Message}", 500);
        }
    }

    /// <summary>Get a doctor by ID.</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DoctorDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDoctorByIdQuery(id), cancellationToken);

        if (result is null)
            return ErrorResponse<DoctorDto>("Doctor not found", 404);

        return SuccessResponse(result, "Doctor retrieved successfully");
    }

    /// <summary>Create a new doctor.</summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<DoctorDto>>> Create([FromBody] CreateDoctorDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateDoctorCommand(dto), cancellationToken);
            return CreatedResponse(result, "Doctor created successfully", new { id = result.Oid });
        }
        catch (Exception ex)
        {
            return ErrorResponse<DoctorDto>($"Error creating doctor: {ex.Message}", 500);
        }
    }

    /// <summary>Update an existing doctor.</summary>
    [HttpPut]
    public async Task<ActionResult<ApiResponse<DoctorDto>>> Update([FromBody] UpdateDoctorDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new UpdateDoctorCommand(dto), cancellationToken);
            return SuccessResponse(result, "Doctor updated successfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ErrorResponse<DoctorDto>(ex.Message, 404);
        }
        catch (Exception ex)
        {
            return ErrorResponse<DoctorDto>($"Error updating doctor: {ex.Message}", 500);
        }
    }

    /// <summary>Delete a doctor by ID.</summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeleteDoctorCommand(id), cancellationToken);

            if (!result)
                return ErrorResponse<bool>("Doctor not found", 404);

            return SuccessResponse(result, "Doctor deleted successfully");
        }
        catch (Exception ex)
        {
            return ErrorResponse<bool>($"Error deleting doctor: {ex.Message}", 500);
        }
    }
}
