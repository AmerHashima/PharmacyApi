using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Auth;

public class RegisterDto
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Mobile { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? MiddleName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    public Guid? Gender { get; set; }
    public string FullName => string.IsNullOrEmpty(MiddleName)
        ? $"{FirstName} {LastName}".Trim()
        : $"{FirstName} {MiddleName} {LastName}".Trim();

    public DateOnly? BirthDate { get; set; }

    public Guid? RoleId { get; set; }  // Default role
}