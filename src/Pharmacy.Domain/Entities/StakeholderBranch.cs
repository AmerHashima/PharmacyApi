using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Junction table linking Stakeholders to Branches.
/// Allows a stakeholder (e.g., supplier) to be associated with multiple branches.
/// </summary>
[Table("StakeholderBranches")]
public class StakeholderBranch : BaseEntity
{
    [Required]
    public Guid StakeholderId { get; set; }

    [ForeignKey(nameof(StakeholderId))]
    public virtual Stakeholder Stakeholder { get; set; } = null!;

    [Required]
    public Guid BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch Branch { get; set; } = null!;

    /// <summary>
    /// Indicates if this is the primary branch for the stakeholder
    /// </summary>
    public bool IsPrimary { get; set; } = false;
}
