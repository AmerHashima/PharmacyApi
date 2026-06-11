using Pharmacy.Api.Models;
using Pharmacy.Api.Localization;
using Pharmacy.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace Pharmacy.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var culture = context.GetRequestCulture();
        var response = exception switch
        {
            Pharmacy.Application.Common.Exceptions.ValidationException validationEx => new ApiResponse
            {
                Success = false,
                MessageEn = JsonStringLocalizer.Get("ValidationFailed", "en") ?? "Validation failed",
                MessageAr = JsonStringLocalizer.Get("ValidationFailed", "ar") ?? "فشل التحقق",
                Message = (culture == "ar") ? JsonStringLocalizer.Get("ValidationFailed", "ar") : JsonStringLocalizer.Get("ValidationFailed", "en"),
                Errors = validationEx.Errors,
                Data = null,
                StatusCode = (int)HttpStatusCode.BadRequest
            },
            BusinessValidationException businessEx => new ApiResponse
            {
                Success = false,
                MessageEn = businessEx.Message,
                MessageAr = businessEx.Message, // fallback - provider should supply Arabic if available
                Message = (culture == "ar") ? businessEx.Message : businessEx.Message,
                Errors = businessEx.PropertyName != null
                    ? new Dictionary<string, string[]> { { businessEx.PropertyName, new[] { businessEx.ErrorMessage ?? businessEx.Message } } }
                    : null,
                Data = null,
                StatusCode = (int)HttpStatusCode.BadRequest
            },
            NotFoundException notFoundEx => new ApiResponse
            {
                Success = false,
                MessageEn = notFoundEx.Message,
                MessageAr = notFoundEx.Message,
                Message = (culture == "ar") ? notFoundEx.Message : notFoundEx.Message,
                Data = null,
                StatusCode = (int)HttpStatusCode.NotFound
            },
            UnauthorizedException unauthorizedEx => new ApiResponse
            {
                Success = false,
                MessageEn = unauthorizedEx.Message,
                MessageAr = unauthorizedEx.Message,
                Message = (culture == "ar") ? unauthorizedEx.Message : unauthorizedEx.Message,
                Data = null,
                StatusCode = (int)HttpStatusCode.Unauthorized
            },
            ForbiddenException forbiddenEx => new ApiResponse
            {
                Success = false,
                MessageEn = forbiddenEx.Message,
                MessageAr = forbiddenEx.Message,
                Message = (culture == "ar") ? forbiddenEx.Message : forbiddenEx.Message,
                Data = null,
                StatusCode = (int)HttpStatusCode.Forbidden
            },
            UnauthorizedAccessException unauthorizedAccessEx => new ApiResponse
            {
                Success = false,
                MessageEn = string.Format(JsonStringLocalizer.Get("FileAccessDenied", "en") ?? "File system access denied: {0}", unauthorizedAccessEx.Message),
                MessageAr = string.Format(JsonStringLocalizer.Get("FileAccessDenied", "ar") ?? "تم رفض الوصول إلى نظام الملفات: {0}", unauthorizedAccessEx.Message),
                Message = (culture == "ar") ? string.Format(JsonStringLocalizer.Get("FileAccessDenied", "ar") ?? "تم رفض الوصول إلى نظام الملفات: {0}", unauthorizedAccessEx.Message) : string.Format(JsonStringLocalizer.Get("FileAccessDenied", "en") ?? "File system access denied: {0}", unauthorizedAccessEx.Message),
                Data = null,
                StatusCode = (int)HttpStatusCode.InternalServerError
            },
            InvalidOperationException invalidOpEx => new ApiResponse
            {
                Success = false,
                MessageEn = invalidOpEx.Message,
                MessageAr = invalidOpEx.Message,
                Message = (culture == "ar") ? invalidOpEx.Message : invalidOpEx.Message,
                Data = null,
                StatusCode = (int)HttpStatusCode.BadRequest
            },
            KeyNotFoundException keyNotFoundEx => new ApiResponse
            {
                Success = false,
                MessageEn = keyNotFoundEx.Message,
                MessageAr = keyNotFoundEx.Message,
                Message = (culture == "ar") ? keyNotFoundEx.Message : keyNotFoundEx.Message,
                Data = null,
                StatusCode = (int)HttpStatusCode.NotFound
            },
            _ => new ApiResponse
            {
                Success = false,
                MessageEn = JsonStringLocalizer.Get("InternalServerError", "en") ?? "An internal server error occurred",
                MessageAr = JsonStringLocalizer.Get("InternalServerError", "ar") ?? "حدث خطأ داخلي في الخادم",
                Message = (culture == "ar") ? JsonStringLocalizer.Get("InternalServerError", "ar") : JsonStringLocalizer.Get("InternalServerError", "en"),
                Data = null,
                StatusCode = (int)HttpStatusCode.InternalServerError
            }
        };

        context.Response.StatusCode = response.StatusCode;
        // Set Content-Language header so clients can pick localized field
        try
        {
            var lang = (context.Request.Headers["Accept-Language"].FirstOrDefault() ?? "en").Split(',').FirstOrDefault()?.Trim();
            if (!string.IsNullOrEmpty(lang)) context.Response.Headers["Content-Language"] = lang.StartsWith("ar", StringComparison.OrdinalIgnoreCase) ? "ar" : "en";
        }
        catch { }
        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}