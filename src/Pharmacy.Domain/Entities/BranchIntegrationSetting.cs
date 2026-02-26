using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents integration settings for a specific branch with a provider
/// </summary>
[Table("BranchIntegrationSettings")]
public class BranchIntegrationSetting : BaseEntity
{
    [Required]
    public Guid IntegrationProviderId { get; set; }

    [Required]
    public Guid BranchId { get; set; }

    [MaxLength(255)]
    public string? IntegrationKey { get; set; }

    [MaxLength(255)]
    public string? IntegrationValue { get; set; }

    /// <summary>
    /// Status: 0=Inactive, 1=Active, 2=Testing
    /// </summary>
    public int Status { get; set; }

    // Navigation Properties
    [ForeignKey(nameof(IntegrationProviderId))]
    public virtual IntegrationProvider IntegrationProvider { get; set; } = null!;

    [ForeignKey(nameof(BranchId))]
    public virtual Branch Branch { get; set; } = null!;
}