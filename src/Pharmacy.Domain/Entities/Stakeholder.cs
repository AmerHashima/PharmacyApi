using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents external business entities that interact with the pharmacy system.
/// Can be: Pharmacy, Supplier, Distributor, Manufacturer, etc.
/// Type is determined by StakeholderTypeId (FK to AppLookupDetail).
/// </summary>
[Table("Stakeholders")]
public class Stakeholder : BaseEntity
{
    /// <summary>
    /// Global Location Number - unique identifier for the stakeholder
    /// </summary>
    [MaxLength(20)]
    public string? GLN { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// FK to AppLookupDetail - determines the type (Pharmacy, Supplier, etc.)
    /// </summary>
    public Guid? StakeholderTypeId { get; set; }

    [ForeignKey(nameof(StakeholderTypeId))]
    public virtual AppLookupDetail? StakeholderType { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(100)]
    public string? District { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(50)]
    public string? ContactPerson { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    // Navigation Properties - Stakeholder can have multiple branches
    public virtual ICollection<StakeholderBranch> StakeholderBranches { get; set; } = new List<StakeholderBranch>();
}
