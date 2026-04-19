namespace Pharmacy.Application.DTOs.Rsd;

/// <summary>
/// Summary record for a product that was updated during the DrugList sync
/// </summary>
public class DrugListSyncResultItemDto
{
    public Guid ProductId { get; set; }
    public string? GTIN { get; set; }
    public string? RegistrationNumber { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public string MatchedBy { get; set; } = string.Empty; // "GTIN" or "RegistrationNumber"
}
