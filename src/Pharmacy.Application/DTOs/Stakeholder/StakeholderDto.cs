namespace Pharmacy.Application.DTOs.Stakeholder;

/// <summary>
/// DTO for reading Stakeholder data
/// </summary>
public class StakeholderDto
{
    public Guid Oid { get; set; }
    public string? GLN { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? StakeholderTypeId { get; set; }
    public string? StakeholderTypeName { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}
