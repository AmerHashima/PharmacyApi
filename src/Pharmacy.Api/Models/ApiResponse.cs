using System.Reflection;
using System.IO;
using System.Linq;

namespace Pharmacy.Api.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    // Backwards-compatible default message (English)
    public string? Message { get; set; }
    // Localized messages
    public string? MessageEn { get; set; }
    public string? MessageAr { get; set; }
    public T? Data { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
    public string? InnerException { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? TraceId { get; set; }

    // Original SuccessResult signature (backwards compatible)
    public static ApiResponse<T> SuccessResult(T data, string? message = null, int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Success = true,
            MessageEn = message ?? "Operation completed successfully",
            MessageAr = message ?? "اكتملت العملية بنجاح",
            Message = message ?? "Operation completed successfully",
            Data = data,
            StatusCode = statusCode
        };
    }

    // Localized variant
    public static ApiResponse<T> SuccessResultLocalized(T data, string? messageEn = null, string? messageAr = null, int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Success = true,
            MessageEn = messageEn ?? "Operation completed successfully",
            MessageAr = messageAr ?? "اكتملت العملية بنجاح",
            Message = messageEn ?? "Operation completed successfully",
            Data = data,
            StatusCode = statusCode
        };
    }

    // Original ErrorResult signatures
    public static ApiResponse<T> ErrorResult(string message, IDictionary<string, string[]>? errors = null, int statusCode = 400, string? innerException = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            MessageEn = message,
            MessageAr = message,
            Message = message,
            Errors = errors,
            StatusCode = statusCode,
            InnerException = innerException
        };
    }

    // Overload for simple error messages (original signature)
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

        return ErrorResult(message, errors, statusCode, innerException);
    }

    // Localized variants
    public static ApiResponse<T> ErrorResultLocalized(string messageEn, string? messageAr = null, IDictionary<string, string[]>? errors = null, int statusCode = 400, string? innerException = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            MessageEn = messageEn,
            MessageAr = messageAr ?? messageEn,
            Message = messageEn,
            Errors = errors,
            StatusCode = statusCode,
            InnerException = innerException
        };
    }

    public static ApiResponse<T> ErrorResultLocalized(string messageEn, string? messageAr = null, List<string>? errorMessages = null, int statusCode = 400, string? innerException = null)
    {
        IDictionary<string, string[]>? errors = null;
        if (errorMessages != null && errorMessages.Any())
        {
            errors = new Dictionary<string, string[]>
            {
                { "General", errorMessages.ToArray() }
            };
        }

        return ErrorResultLocalized(messageEn, messageAr, errors, statusCode, innerException);
    }

    // Exception overloads
    public static ApiResponse<T> ErrorResult(Exception exception, int statusCode = 500)
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

        return new ApiResponse<T>
        {
            Success = false,
            MessageEn = "An error occurred while processing your request",
            MessageAr = "حدث خطأ أثناء معالجة طلبك",
            Message = "An error occurred while processing your request",
            Errors = errors,
            StatusCode = statusCode,
            InnerException = exception.InnerException?.Message
        };
    }

    public static ApiResponse<T> ErrorResultLocalized(string messageEn, string? messageAr, Exception exception, int statusCode = 500)
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

        return new ApiResponse<T>
        {
            Success = false,
            MessageEn = messageEn,
            MessageAr = messageAr ?? messageEn,
            Message = messageEn,
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
            MessageEn = message ?? "Operation completed successfully",
            MessageAr = message ?? "اكتملت العملية بنجاح",
            Message = message ?? "Operation completed successfully",
            StatusCode = statusCode
        };
    }

    public new static ApiResponse ErrorResult(string message, IDictionary<string, string[]>? errors = null, int statusCode = 400, string? innerException = null)
    {
        return new ApiResponse
        {
            Success = false,
            MessageEn = message,
            MessageAr = message,
            Message = message,
            Errors = errors,
            StatusCode = statusCode,
            InnerException = innerException
        };
    }

    public static new ApiResponse ErrorResult(string message, List<string>? errorMessages = null, int statusCode = 400, string? innerException = null)
    {
        IDictionary<string, string[]>? errors = null;
        if (errorMessages != null && errorMessages.Any())
        {
            errors = new Dictionary<string, string[]>
            {
                { "General", errorMessages.ToArray() }
            };
        }

        return ErrorResult(message, errors, statusCode, innerException);
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
            MessageEn = "An error occurred while processing your request",
            MessageAr = "حدث خطأ أثناء معالجة طلبك",
            Message = "An error occurred while processing your request",
            Errors = errors,
            StatusCode = statusCode,
            InnerException = exception.InnerException?.Message
        };
    }
}
