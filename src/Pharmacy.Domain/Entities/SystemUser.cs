using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

[Table("SystemUsers")]
public class SystemUser : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(256)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(128)]
    public string PasswordSalt { get; set; } = string.Empty;

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

    public string FullName { get; set; } = string.Empty;

    // Foreign Key for Gender
    public Guid? GenderLookupId { get; set; }
    [ForeignKey(nameof(GenderLookupId))]
    public virtual AppLookupDetail? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    // Foreign Key for Role
    //[Required]
    public Guid? RoleId { get; set; }
    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; set; } = null!;

    // Foreign Key for Branch - User belongs to a specific branch
    public Guid? BranchId { get; set; }
    [ForeignKey(nameof(BranchId))]
    public virtual Branch? Branch { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? LastLogin { get; set; }

    public int FailedLoginCount { get; set; } = 0;

    public DateTime? LockoutEnd { get; set; }

    public DateTime? PasswordExpiry { get; set; }

    public bool TwoFactorEnabled { get; set; } = false;

    // Navigation Properties
    public virtual ICollection<SalesInvoice> ProcessedInvoices { get; set; } = new List<SalesInvoice>();
}