using System.Reflection;

namespace Pharmacy.Api.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; } // Fixed: Changed from List<string> to IDictionary
    public string? InnerException { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? TraceId { get; set; }

    public static ApiResponse<T> SuccessResult(T data, string? message = null, int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message ?? "Operation completed successfully",
            Data = data,
            StatusCode = statusCode
        };
    }

    public static ApiResponse<T> ErrorResult(string message, IDictionary<string, string[]>? errors = null, int statusCode = 400, string? innerException = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors,
            StatusCode = statusCode,
            InnerException = innerException
        };
    }

    // Overload for simple error messages
    public static ApiResponse<T> ErrorResult(string message, List<string>? errorMessages = null, int statusCode = 400, string? innerException = null)
    {
        IDictionary<string, string[]>? errors = null;
        if (errorMessages != null && errorMessages.Any())
        {
            errors = new Dictionary<string, string[]>
            {
                { "General", errorMessages.ToArray() }
            };
        }

        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors,
            StatusCode = statusCode,
            InnerException = innerException
        };
    }

    public static ApiResponse<T> ErrorResult(Exception exception, int statusCode = 500)
    {
        var errors = new Dictionary<string, string[]>
        {
            { "Exception", new[] { exception.Message } }
        };

        // Handle specific exception types
        if (exception is ReflectionTypeLoadException reflectionEx && reflectionEx.LoaderExceptions != null)
        {
            var loaderErrors = reflectionEx.LoaderExceptions
                .Where(ex => ex != null)
                .Select(ex => ex!.Message)
                .ToArray();

            if (loaderErrors.Any())
            {
                errors["LoaderExceptions"] = loaderErrors;
            }
        }

        return new ApiResponse<T>
        {
            Success = false,
            Message = "An error occurred while processing your request",
            Errors = errors,
            StatusCode = statusCode,
            InnerException = exception.InnerException?.Message
        };
    }
}

public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse SuccessResult(string? message = null, int statusCode = 200)
    {
        return new ApiResponse
        {
            Success = true,
            Message = message ?? "Operation completed successfully",
            StatusCode = statusCode
        };
    }

    public new static ApiResponse ErrorResult(string message, IDictionary<string, string[]>? errors = null, int statusCode = 400, string? innerException = null)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            Errors = errors,
            StatusCode = statusCode,
            InnerException = innerException
        };
    }

    // Overload for simple error messages
    public static ApiResponse ErrorResult(string message, List<string>? errorMessages, int statusCode = 400, string? innerException = null)
    {
        IDictionary<string, string[]>? errors = null;
        if (errorMessages != null && errorMessages.Any())
        {
            errors = new Dictionary<string, string[]>
            {
                { "General", errorMessages.ToArray() }
            };
        }

        return new ApiResponse
        {
            Success = false,
            Message = message,
            Errors = errors,
            StatusCode = statusCode,
            InnerException = innerException
        };
    }

    public new static ApiResponse ErrorResult(Exception exception, int statusCode = 500)
    {
        var errors = new Dictionary<string, string[]>
        {
            { "Exception", new[] { exception.Message } }
        };

        if (exception is ReflectionTypeLoadException reflectionEx && reflectionEx.LoaderExceptions != null)
        {
            var loaderErrors = reflectionEx.LoaderExceptions
                .Where(ex => ex != null)
                .Select(ex => ex!.Message)
                .ToArray();

            if (loaderErrors.Any())
            {
                errors["LoaderExceptions"] = loaderErrors;
            }
        }

        return new ApiResponse
        {
            Success = false,
            Message = "An error occurred while processing your request",
            Errors = errors,
            StatusCode = statusCode,
            InnerException = exception.InnerException?.Message
        };
    }
}