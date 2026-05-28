namespace Pharmacy.Application.DTOs.Accounting;

/// <summary>
/// Result of a branch accounting setup validation check.
/// </summary>
public class AccountingValidationResultDto
{
    public bool IsValid { get; set; }
    public string OperationType { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
    public List<string> MissingAccounts { get; set; } = [];
    public string? Message { get; set; }
}
