namespace Pharmacy.Application.DTOs.GenericName;

public class GenericNameDto
{
    public Guid Oid { get; set; }
    public string NameEN { get; set; } = string.Empty;
    public string NameAR { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
}
