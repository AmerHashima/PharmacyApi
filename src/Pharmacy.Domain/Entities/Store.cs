using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Represents a physical or logical store/warehouse within a branch.
/// A branch can have multiple stores; one can be set as the default.
/// </summary>
[Table("Stores")]
public class Store : BaseEntity
{
    /// <summary>
    /// Unique store code identifier
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string StoreCode { get; set; } = string.Empty;

    /// <summary>
    /// Display name of the store
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string StoreName { get; set; } = string.Empty;

    /// <summary>
    /// Optional description or purpose of the store
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Physical address of the store
    /// </summary>
    [MaxLength(500)]
    public string? Address { get; set; }

    /// <summary>
    /// Contact phone number
    /// </summary>
    [MaxLength(50)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Whether this store is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// FK to Branch — the branch this store belongs to
    /// </summary>
    [Required]
    public Guid BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch Branch { get; set; } = null!;
}
