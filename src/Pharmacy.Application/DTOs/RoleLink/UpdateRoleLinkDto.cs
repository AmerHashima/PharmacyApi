using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.RoleLink;

public class UpdateRoleLinkDto
{
    [Required]
    public Guid Oid { get; set; }

    public bool CanRead   { get; set; }
    public bool CanWrite  { get; set; }
    public bool CanEdit   { get; set; }
    public bool CanDelete { get; set; }
}
