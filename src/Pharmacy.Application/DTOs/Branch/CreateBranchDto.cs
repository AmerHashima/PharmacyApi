using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Branch;

/// <summary>
/// DTO for creating a new Branch
/// </summary>
public class CreateBranchDto
{
    [Required(ErrorMessage = "Branch code is required")]
    [MaxLength(50, ErrorMessage = "Branch code cannot exceed 50 characters")]
    public string BranchCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Branch name is required")]
    [MaxLength(200, ErrorMessage = "Branch name cannot exceed 200 characters")]
    public string BranchName { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "GLN cannot exceed 20 characters")]
    public string? GLN { get; set; }

    [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    public string? City { get; set; }

    [MaxLength(100, ErrorMessage = "District cannot exceed 100 characters")]
    public string? District { get; set; }

    [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? Address { get; set; }
    
    public int? Status { get; set; }
}
