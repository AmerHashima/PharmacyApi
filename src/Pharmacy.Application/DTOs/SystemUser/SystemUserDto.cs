namespace Pharmacy.Application.DTOs.SystemUserSpace;

public class SystemUserDto
{
    public Guid Oid { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;

    public Guid? GenderLookupId { get; set; }
    public string? GenderName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public Guid RoleId { get; set; }
    public string? RoleName { get; set; }

    public bool IsActive { get; set; }
    public DateTime? LastLogin { get; set; }
    public int FailedLoginCount { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public DateTime? PasswordExpiry { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}