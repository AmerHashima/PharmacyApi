namespace Pharmacy.Application.DTOs.Accounting;

public class JournalEntryDetailDto
{
    public Guid Oid { get; set; }
    public Guid JournalEntryId { get; set; }
    public Guid AccountId { get; set; }
    public string? AccountCode { get; set; }
    public string? AccountNameAr { get; set; }
    public Guid? CostCenterId { get; set; }
    public string? CostCenterNameAr { get; set; }
    public string? Description { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public int LineNumber { get; set; }

}

public class JournalEntryMasterDto
{
    public Guid Oid { get; set; }
    public string EntryNumber { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; }
    public Guid? FiscalYearId { get; set; }
    public string? FiscalYearName { get; set; }
    public Guid? BranchId { get; set; }
    public string? BranchName { get; set; }
    public string? Description { get; set; }
    public Guid? ReferenceId { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public bool IsPosted { get; set; }
    public bool IsReversed { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class JournalEntryDto
{
    public Guid Oid { get; set; }
    public string EntryNumber { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; }
    public Guid? FiscalYearId { get; set; }
    public string? FiscalYearName { get; set; }
    public Guid? BranchId { get; set; }
    public string? BranchName { get; set; }
    public string? Description { get; set; }
    public Guid? ReferenceTypeId { get; set; }
    public string? ReferenceTypeName { get; set; }
    public Guid? ReferenceId { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public bool IsPosted { get; set; }
    public bool IsReversed { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<JournalEntryDetailDto> Details { get; set; } = new();
}

public class CreateJournalEntryDetailDto
{
    public Guid AccountId { get; set; }
    public Guid? CostCenterId { get; set; }
    public string? Description { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
}

public class CreateJournalEntryDto
{
    public string EntryNumber { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; }
    public Guid? FiscalYearId { get; set; }
    public Guid? BranchId { get; set; }
    public string? Description { get; set; }
    public Guid? ReferenceTypeId { get; set; }
    public Guid? ReferenceId { get; set; }
    public List<CreateJournalEntryDetailDto> Details { get; set; } = new();
}

public class UpdateJournalEntryDetailDto
{
    public Guid? Oid { get; set; }
    public Guid AccountId { get; set; }
    public Guid? CostCenterId { get; set; }
    public string? Description { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
}

public class UpdateJournalEntryDto
{
    public Guid Oid { get; set; }
    public string EntryNumber { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; }
    public Guid? FiscalYearId { get; set; }
    public Guid? BranchId { get; set; }
    public string? Description { get; set; }
    public Guid? ReferenceTypeId { get; set; }
    public Guid? ReferenceId { get; set; }
    public bool IsPosted { get; set; }
    public bool IsReversed { get; set; }
    public List<UpdateJournalEntryDetailDto> Details { get; set; } = new();
}
