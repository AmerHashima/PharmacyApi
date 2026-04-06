namespace Pharmacy.Application.DTOs.Store;

/// <summary>
/// DTO for reading Store data
/// </summary>
public class StoreDto
{
    public Guid Oid { get; set; }
    public string StoreCode { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public Guid BranchId { get; set; }
    public string? BranchName { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}
