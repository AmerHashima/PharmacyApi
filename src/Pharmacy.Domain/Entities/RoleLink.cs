using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

/// <summary>
/// Junction table linking a Role to a Link (screen / report) with per-permission flags.
/// </summary>
[Table("RoleLinks")]
public class RoleLink : BaseEntity
{
    // ── Foreign Keys ──────────────────────────────────────────────────────

    public Guid RoleId { get; set; }
    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; set; }

    public Guid LinkId { get; set; }
    [ForeignKey(nameof(LinkId))]
    public virtual Link? Link { get; set; }

    // ── Permission Flags ──────────────────────────────────────────────────

    public bool CanRead   { get; set; } = false;
    public bool CanWrite  { get; set; } = false;
    public bool CanEdit   { get; set; } = false;
    public bool CanDelete { get; set; } = false;
}
