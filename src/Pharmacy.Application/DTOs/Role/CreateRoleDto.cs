using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Role;

public class CreateRoleDto
{
    [Required(ErrorMessage = "Role name is required")]
    [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
    public string? Description { get; set; }
}