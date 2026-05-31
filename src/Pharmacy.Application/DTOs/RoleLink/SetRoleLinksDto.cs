using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.RoleLink;

/// <summary>
/// Upsert all link permissions for a role in a single call.
/// Any existing RoleLink entries for the role are replaced.
/// </summary>
public class SetRoleLinksDto
{
    [Required]
    public Guid RoleId { get; set; }

    [Required]
    public List<RoleLinkEntryDto> Links { get; set; } = new();
}

public class RoleLinkEntryDto
{
    [Required]
    public Guid LinkId { get; set; }

    public bool CanRead   { get; set; } = false;
    public bool CanWrite  { get; set; } = false;
    public bool CanEdit   { get; set; } = false;
    public bool CanDelete { get; set; } = false;
}
