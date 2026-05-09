namespace Pharmacy.Application.DTOs.Accounting;

public class BankAccountDto
{
    public Guid Oid { get; set; }
    public string? Code { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public string? AccountNumber { get; set; }
    public string? IBAN { get; set; }
    public Guid? BranchId { get; set; }
    public string? BranchName { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountNameAr { get; set; }
    public Guid? CurrencyCodeId { get; set; }
    public string? CurrencyCodeName { get; set; }
    public bool IsActive { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class CreateBankAccountDto
{
    public string? Code { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public string? AccountNumber { get; set; }
    public string? IBAN { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? CurrencyCodeId { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateBankAccountDto
{
    public Guid Oid { get; set; }
    public string? Code { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public string? AccountNumber { get; set; }
    public string? IBAN { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? CurrencyCodeId { get; set; }
    public bool IsActive { get; set; }
}
