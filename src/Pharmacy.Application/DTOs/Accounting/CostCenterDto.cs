namespace Pharmacy.Application.DTOs.Accounting;

public class CostCenterDto
{
    public Guid Oid { get; set; }
    public string? Code { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public Guid? ParentId { get; set; }
    public string? ParentNameAr { get; set; }
    public bool IsActive { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class CreateCostCenterDto
{
    public string? Code { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateCostCenterDto
{
    public Guid Oid { get; set; }
    public string? Code { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsActive { get; set; }
}
