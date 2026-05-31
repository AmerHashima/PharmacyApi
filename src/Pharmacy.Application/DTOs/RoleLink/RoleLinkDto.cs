namespace Pharmacy.Application.DTOs.RoleLink;

public class RoleLinkDto
{
    public Guid Oid       { get; set; }
    public Guid RoleId    { get; set; }
    public string? RoleName { get; set; }
    public Guid LinkId    { get; set; }
    public string? LinkNameAr { get; set; }
    public string? LinkNameEn { get; set; }
    public string? LinkPath   { get; set; }
    public bool CanRead   { get; set; }
    public bool CanWrite  { get; set; }
    public bool CanEdit   { get; set; }
    public bool CanDelete { get; set; }
}
