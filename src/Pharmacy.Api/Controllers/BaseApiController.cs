using Pharmacy.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Localization;

namespace Pharmacy.Api.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected ActionResult<ApiResponse<T>> SuccessResponse<T>(T data, string? message = null)
    {
        var response = ApiResponse<T>.SuccessResult(data, message);
        // try to set localized messageAr if possible using resource key
        var key = JsonStringLocalizer.FindKeyForValue(message ?? "Operation completed successfully", "en");
        if (key != null)
        {
            response.MessageAr = JsonStringLocalizer.Get(key, "ar");
            response.MessageEn = JsonStringLocalizer.Get(key, "en");
        }
        var culture = HttpContext.GetRequestCulture();
        // set Message to selected culture for backward compatibility
        response.Message = culture == "ar" ? response.MessageAr ?? response.MessageEn : response.MessageEn ?? response.MessageAr ?? response.Message;
        response.TraceId = HttpContext.TraceIdentifier;
        Response.Headers["Content-Language"] = culture;
        return Ok(response);
    }

    protected ActionResult<ApiResponse<T>> CreatedResponse<T>(T data, string actionName, object routeValues, string? message = null)
    {
        var response = ApiResponse<T>.SuccessResult(data, message ?? "Resource created successfully", 201);
        var key = JsonStringLocalizer.FindKeyForValue(message ?? "Resource created successfully", "en");
        if (key != null)
        {
            response.MessageAr = JsonStringLocalizer.Get(key, "ar");
            response.MessageEn = JsonStringLocalizer.Get(key, "en");
        }
        var culture = HttpContext.GetRequestCulture();
        response.Message = culture == "ar" ? response.MessageAr ?? response.MessageEn : response.MessageEn ?? response.MessageAr ?? response.Message;
        response.TraceId = HttpContext.TraceIdentifier;
        Response.Headers["Content-Language"] = culture;
        return CreatedAtAction(actionName, routeValues, response);
    }

    protected ActionResult<ApiResponse> SuccessResponse(string? message = null)
    {
        var response = ApiResponse.SuccessResult(message);
        var key = JsonStringLocalizer.FindKeyForValue(message ?? "Operation completed successfully", "en");
        if (key != null)
        {
            response.MessageAr = JsonStringLocalizer.Get(key, "ar");
            response.MessageEn = JsonStringLocalizer.Get(key, "en");
        }
        var culture = HttpContext.GetRequestCulture();
        response.Message = culture == "ar" ? response.MessageAr ?? response.MessageEn : response.MessageEn ?? response.MessageAr ?? response.Message;
        response.TraceId = HttpContext.TraceIdentifier;
        Response.Headers["Content-Language"] = culture;
        return Ok(response);
    }

    protected ActionResult<ApiResponse> NoContentResponse()
    {
        var response = ApiResponse.SuccessResult("Resource deleted successfully", 204);
        var key = JsonStringLocalizer.FindKeyForValue("Resource deleted successfully", "en");
        if (key != null)
        {
            response.MessageAr = JsonStringLocalizer.Get(key, "ar");
            response.MessageEn = JsonStringLocalizer.Get(key, "en");
        }
        var culture = HttpContext.GetRequestCulture();
        response.Message = culture == "ar" ? response.MessageAr ?? response.MessageEn : response.MessageEn ?? response.MessageAr ?? response.Message;
        response.TraceId = HttpContext.TraceIdentifier;
        Response.Headers["Content-Language"] = culture;
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