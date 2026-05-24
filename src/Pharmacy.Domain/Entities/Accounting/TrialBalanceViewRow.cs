namespace Pharmacy.Domain.Entities.Accounting;

/// <summary>
/// Keyless entity mapped to [Accounting].[vw_TrialBalanceTree].
/// One row per journal entry detail line, with account tree fields denormalized.
/// </summary>
public class TrialBalanceViewRow
{
    public Guid Oid { get; set; }
    public Guid? ParentId { get; set; }
    public string? ParentCode { get; set; }
    public string? ParentNameAr { get; set; }

    public string AccountCode { get; set; } = string.Empty;
    public string AccountNameAr { get; set; } = string.Empty;
    public string? AccountNameEn { get; set; }
    public int AccountLevel { get; set; }
    public bool IsLeaf { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string TreePath { get; set; } = string.Empty;

    public Guid? BranchId { get; set; }
    public string? BranchNameAr { get; set; }
    public string? BranchNameEn { get; set; }

    public Guid? CostCenterId { get; set; }
    public string? CostCenterNameAr { get; set; }
    public string? CostCenterNameEn { get; set; }

    public DateTime? EntryDate { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
}
