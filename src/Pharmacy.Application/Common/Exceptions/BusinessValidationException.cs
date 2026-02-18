namespace Pharmacy.Application.Common.Exceptions;

public class BusinessValidationException : Exception
{
    public BusinessValidationException(string message)
        : base(message)
    {
    }

    public BusinessValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public BusinessValidationException(string propertyName, string errorMessage)
        : base($"Business validation failed for {propertyName}: {errorMessage}")
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
    }

    public string? PropertyName { get; }
    public string? ErrorMessage { get; }
}