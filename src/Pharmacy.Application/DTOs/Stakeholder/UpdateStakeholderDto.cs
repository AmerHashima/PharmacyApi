using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Stakeholder;

/// <summary>
/// DTO for updating an existing Stakeholder
/// </summary>
public class UpdateStakeholderDto
{
    [Required]
    public Guid Oid { get; set; }

    [MaxLength(20, ErrorMessage = "GLN cannot exceed 20 characters")]
    public string? GLN { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    public Guid? StakeholderTypeId { get; set; }

    [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    public string? City { get; set; }

    [MaxLength(100, ErrorMessage = "District cannot exceed 100 characters")]
    public string? District { get; set; }

    [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(50, ErrorMessage = "Contact person cannot exceed 50 characters")]
    public string? ContactPerson { get; set; }

    [MaxLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string? Email { get; set; }
    
    public int? Status { get; set; }
}
