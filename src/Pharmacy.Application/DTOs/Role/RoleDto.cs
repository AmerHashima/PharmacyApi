namespace Pharmacy.Application.DTOs.Role;

public class RoleDto
{
    public Guid Oid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}