namespace Pharmacy.Application.DTOs.Accounting;

// ─────────────────────────────────────────────────────────────────────────────
// Trial Balance — Request
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Parameters for generating a Trial Balance (ميزان المراجعة).
/// All list parameters support multi-select (empty list = no filter = all).
/// </summary>
public class TrialBalanceRequest
{
    /// <summary>Period start date (inclusive).</summary>
    public DateTime FromDate { get; set; }

    /// <summary>Period end date (inclusive).</summary>
    public DateTime ToDate { get; set; }

    /// <summary>
    /// Multi-select branch filter.
    /// Empty = all branches.
    /// </summary>
    public List<Guid> BranchIds { get; set; } = [];

    /// <summary>
    /// Multi-select cost center filter.
    /// Empty = all cost centers (including lines with no cost center).
    /// </summary>
    public List<Guid> CostCenterIds { get; set; } = [];

    /// <summary>
    /// Multi-select specific accounts filter.
    /// Empty = all accounts.
    /// </summary>
    public List<Guid> AccountIds { get; set; } = [];

    /// <summary>
    /// Filter to a specific account subtree.
    /// Null = entire chart of accounts.
    /// </summary>
    public Guid? ParentAccountId { get; set; }

    /// <summary>
    /// true  → leaf accounts only (posting accounts).
    /// false → summary accounts only.
    /// null  → all levels.
    /// </summary>
    public bool? IsLeafOnly { get; set; }

    /// <summary>
    /// When true, exclude rows where all columns are zero
    /// (opening + period movement + closing all = 0).
    /// </summary>
    public bool RemoveZeroBalance { get; set; } = false;

    /// <summary>Minimum account level to include (1 = root). Default 1.</summary>
    public int MinLevel { get; set; } = 1;

    /// <summary>Maximum account level to include. Null = no upper limit.</summary>
    public int? MaxLevel { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// Trial Balance — Row result
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// One row in the Trial Balance report (ميزان المراجعة).
/// When BranchIds or CostCenterIds filters are applied, each account may appear
/// once per branch/cost center combination.
/// </summary>
public class TrialBalanceRowDto
{
    // ── Account identity ──────────────────────────────────────────────────────
    public Guid Oid { get; set; }
    public Guid? ParentId { get; set; }
    public string? ParentCode { get; set; }
    public string? ParentNameAr { get; set; }
    public string? ParentNameEn { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountNameAr { get; set; } = string.Empty;
    public string? AccountNameEn { get; set; }
    public int AccountLevel { get; set; }
    public bool IsLeaf { get; set; }

    /// <summary>Indented Arabic name for tree display.</summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>Dot-separated code path for sorting, e.g. "1.1.2".</summary>
    public string TreePath { get; set; } = string.Empty;

    // ── Branch (present when filtered by branch) ──────────────────────────────
    public Guid? BranchId { get; set; }
    public string? BranchNameAr { get; set; }
    public string? BranchNameEn { get; set; }

    // ── Cost Center (present when filtered by cost center) ────────────────────
    public Guid? CostCenterId { get; set; }
    public string? CostCenterNameAr { get; set; }
    public string? CostCenterNameEn { get; set; }

    // ── Opening balance (before FromDate) ────────────────────────────────────
    public decimal OpeningDebit { get; set; }
    public decimal OpeningCredit { get; set; }

    // ── Period movement (FromDate .. ToDate inclusive) ────────────────────────
    public decimal PeriodDebit { get; set; }
    public decimal PeriodCredit { get; set; }

    // ── Closing balance ───────────────────────────────────────────────────────
    public decimal ClosingDebit { get; set; }
    public decimal ClosingCredit { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// Trial Balance — Report envelope
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Full Trial Balance report result.</summary>
public class TrialBalanceReportDto
{
    public IReadOnlyList<TrialBalanceRowDto> Rows { get; set; } = [];

    // ── Grand totals ──────────────────────────────────────────────────────────
    public decimal TotalOpeningDebit { get; set; }
    public decimal TotalOpeningCredit { get; set; }
    public decimal TotalPeriodDebit { get; set; }
    public decimal TotalPeriodCredit { get; set; }
    public decimal TotalClosingDebit { get; set; }
    public decimal TotalClosingCredit { get; set; }

    // ── Meta ──────────────────────────────────────────────────────────────────
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public List<Guid> BranchIds { get; set; } = [];
    public List<Guid> CostCenterIds { get; set; } = [];
    public List<Guid> AccountIds { get; set; } = [];
    public int RowCount { get; set; }
}

