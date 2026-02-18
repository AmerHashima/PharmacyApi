using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.SystemUserSpace;

public class CreateSystemUserDto
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    public string Password { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Invalid mobile number format")]
    [StringLength(20, ErrorMessage = "Mobile number cannot exceed 20 characters")]
    public string? Mobile { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Middle name cannot exceed 50 characters")]
    public string? MiddleName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; } = string.Empty;

    public Guid? GenderLookupId { get; set; }
    public DateOnly? BirthDate { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public Guid RoleId { get; set; }

    public bool IsActive { get; set; } = true;
    public bool TwoFactorEnabled { get; set; } = false;
}