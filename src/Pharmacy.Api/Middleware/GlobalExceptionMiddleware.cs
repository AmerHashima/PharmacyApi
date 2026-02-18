using Pharmacy.Api.Models;
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

        var response = exception switch
        {
            Pharmacy.Application.Common.Exceptions.ValidationException validationEx => new ApiResponse
            {
                Success = false,
                Message = "Validation failed",
                Errors = validationEx.Errors,
                Data = null,
                StatusCode = (int)HttpStatusCode.BadRequest
            },
            BusinessValidationException businessEx => new ApiResponse
            {
                Success = false,
                Message = businessEx.Message,
                Errors = businessEx.PropertyName != null
                    ? new Dictionary<string, string[]> { { businessEx.PropertyName, new[] { businessEx.ErrorMessage ?? businessEx.Message } } }
                    : null,
                Data = null,
                StatusCode = (int)HttpStatusCode.BadRequest
            },
            NotFoundException notFoundEx => new ApiResponse
            {
                Success = false,
                Message = notFoundEx.Message,
                Data = null,
                StatusCode = (int)HttpStatusCode.NotFound
            },
            UnauthorizedException unauthorizedEx => new ApiResponse
            {
                Success = false,
                Message = unauthorizedEx.Message,
                Data = null,
                StatusCode = (int)HttpStatusCode.Unauthorized
            },
            ForbiddenException forbiddenEx => new ApiResponse
            {
                Success = false,
                Message = forbiddenEx.Message,
                Data = null,
                StatusCode = (int)HttpStatusCode.Forbidden
            },
            UnauthorizedAccessException => new ApiResponse
            {
                Success = false,
                Message = "Unauthorized access",
                Data = null,
                StatusCode = (int)HttpStatusCode.Unauthorized
            },
            InvalidOperationException invalidOpEx => new ApiResponse
            {
                Success = false,
                Message = invalidOpEx.Message,
                Data = null,
                StatusCode = (int)HttpStatusCode.BadRequest
            },
            KeyNotFoundException keyNotFoundEx => new ApiResponse
            {
                Success = false,
                Message = keyNotFoundEx.Message,
                Data = null,
                StatusCode = (int)HttpStatusCode.NotFound
            },
            _ => new ApiResponse
            {
                Success = false,
                Message = "An internal server error occurred",
                Data = null,
                StatusCode = (int)HttpStatusCode.InternalServerError
            }
        };

        context.Response.StatusCode = response.StatusCode;
        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}