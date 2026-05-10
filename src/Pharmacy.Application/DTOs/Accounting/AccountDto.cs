namespace Pharmacy.Application.DTOs.Accounting;

public class AccountDto
{
    public Guid Oid { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountNameAr { get; set; } = string.Empty;
    public string? AccountNameEn { get; set; }
    public Guid? ParentId { get; set; }
    public string? ParentNameAr { get; set; }
    public int AccountLevel { get; set; }
    public Guid? AccountTypeId { get; set; }
    public string? AccountTypeName { get; set; }
    public string? AccountTypeNameAr { get; set; }
    public Guid? NatureId { get; set; }
    public string? NatureName { get; set; }
    public string? NatureNameAr { get; set; }
    public Guid? FinalAccountId { get; set; }
    public string? FinalAccountName { get; set; }
    public string? FinalAccountNameAr { get; set; }
    public bool IsLeaf { get; set; }
    public bool IsActive { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class CreateAccountDto
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountNameAr { get; set; } = string.Empty;
    public string? AccountNameEn { get; set; }
    public Guid? ParentId { get; set; }
    public int AccountLevel { get; set; } = 1;
    public Guid? AccountTypeId { get; set; }
    public Guid? NatureId { get; set; }
    public Guid? FinalAccountId { get; set; }
    public bool IsLeaf { get; set; } = true;
    public bool IsActive { get; set; } = true;
}

public class UpdateAccountDto
{
    public Guid Oid { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountNameAr { get; set; } = string.Empty;
    public string? AccountNameEn { get; set; }
    public Guid? ParentId { get; set; }
    public int AccountLevel { get; set; }
    public Guid? AccountTypeId { get; set; }
    public Guid? NatureId { get; set; }
    public Guid? FinalAccountId { get; set; }
    public bool IsLeaf { get; set; }
    public bool IsActive { get; set; }
}
