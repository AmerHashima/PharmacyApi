using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.RoleLink;

public class CreateRoleLinkDto
{
    [Required]
    public Guid RoleId { get; set; }

    [Required]
    public Guid LinkId { get; set; }

    public bool CanRead   { get; set; } = false;
    public bool CanWrite  { get; set; } = false;
    public bool CanEdit   { get; set; } = false;
    public bool CanDelete { get; set; } = false;
}
