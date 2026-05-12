namespace Pharmacy.Application.DTOs.Accounting;

/// <summary>
/// Flat report DTO — JournalEntryDetail is the main row,
/// joined with JournalEntry header and all related lookup tables.
/// </summary>
public class JournalEntryDetailReportDto
{
    // ── Detail row ────────────────────────────────────────────
    public Guid Oid { get; set; }
    public Guid JournalEntryId { get; set; }
    public string? Description { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public DateTime? CreatedAt { get; set; }

    // ── Account ───────────────────────────────────────────────
    public Guid AccountId { get; set; }
    public string? AccountCode { get; set; }
    public string? AccountNameAr { get; set; }
    public string? AccountNameEn { get; set; }

    // ── CostCenter ────────────────────────────────────────────
    public Guid? CostCenterId { get; set; }
    public string? CostCenterCode { get; set; }
    public string? CostCenterNameAr { get; set; }

    // ── JournalEntry (header) ─────────────────────────────────
    public string? EntryNumber { get; set; }
    public DateTime EntryDate { get; set; }
    public string? EntryDescription { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public bool IsPosted { get; set; }
    public bool IsReversed { get; set; }
    public Guid? ReferenceId { get; set; }

    // ── FiscalYear ────────────────────────────────────────────
    public Guid? FiscalYearId { get; set; }
    public string? FiscalYearNameAr { get; set; }
    public string? FiscalYearNameEn { get; set; }

    // ── Branch ────────────────────────────────────────────────
    public Guid? BranchId { get; set; }
    public string? BranchName { get; set; }

    // ── ReferenceType (AppLookupDetail) ───────────────────────
    public Guid? ReferenceTypeId { get; set; }
    public string? ReferenceTypeName { get; set; }
    public string? ReferenceTypeNameAr { get; set; }
}
