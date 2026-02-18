using Pharmacy.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Api.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<ApiResponse<T>> SuccessResponse<T>(T data, string? message = null)
    {
        var response = ApiResponse<T>.SuccessResult(data, message);
        response.TraceId = HttpContext.TraceIdentifier;
        return Ok(response);
    }

    protected ActionResult<ApiResponse<T>> CreatedResponse<T>(T data, string actionName, object routeValues, string? message = null)
    {
        var response = ApiResponse<T>.SuccessResult(data, message ?? "Resource created successfully", 201);
        response.TraceId = HttpContext.TraceIdentifier;
        return CreatedAtAction(actionName, routeValues, response);
    }

    protected ActionResult<ApiResponse> SuccessResponse(string? message = null)
    {
        var response = ApiResponse.SuccessResult(message);
        response.TraceId = HttpContext.TraceIdentifier;
        return Ok(response);
    }

    protected ActionResult<ApiResponse> NoContentResponse()
    {
        var response = ApiResponse.SuccessResult("Resource deleted successfully", 204);
        response.TraceId = HttpContext.TraceIdentifier;
        return NoContent();
    }

    protected ActionResult<ApiResponse<T>> ErrorResponse<T>(string message, int statusCode = 400, List<string>? errors = null)
    {
        var response = ApiResponse<T>.ErrorResult(message, errors, statusCode);
        response.TraceId = HttpContext.TraceIdentifier;
        return StatusCode(statusCode, response);
    }

    protected ActionResult<ApiResponse> ErrorResponse(string message, int statusCode = 400, List<string>? errors = null)
    {
        var response = ApiResponse.ErrorResult(message, errors, statusCode);
        response.TraceId = HttpContext.TraceIdentifier;
        return StatusCode(statusCode, response);
    }
}