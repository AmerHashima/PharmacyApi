using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents an external integration provider (e.g., payment gateways, ERP systems, etc.)
/// </summary>
[Table("IntegrationProviders")]
public class IntegrationProvider : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Description { get; set; }

    /// <summary>
    /// Status: 0=Inactive, 1=Active, 2=Suspended
    /// </summary>
    public int Status { get; set; }

    // Navigation Properties
    public virtual ICollection<BranchIntegrationSetting> BranchSettings { get; set; } = new List<BranchIntegrationSetting>();
}