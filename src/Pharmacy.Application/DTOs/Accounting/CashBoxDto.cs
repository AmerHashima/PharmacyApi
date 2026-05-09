namespace Pharmacy.Application.DTOs.Accounting;

public class CashBoxDto
{
    public Guid Oid { get; set; }
    public string? Code { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public Guid? BranchId { get; set; }
    public string? BranchName { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountNameAr { get; set; }
    public bool IsActive { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class CreateCashBoxDto
{
    public string? Code { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? AccountId { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateCashBoxDto
{
    public Guid Oid { get; set; }
    public string? Code { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? AccountId { get; set; }
    public bool IsActive { get; set; }
}
