namespace Pharmacy.Application.DTOs.Accounting;

// ─────────────────────────────────────────────────────────────────────────────
// Trial Balance — Request
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Parameters for generating a Trial Balance (ميزان المراجعة).
/// </summary>
public class TrialBalanceRequest
{
    /// <summary>Period start date (inclusive).</summary>
    public DateTime FromDate { get; set; }

    /// <summary>Period end date (inclusive).</summary>
    public DateTime ToDate { get; set; }

    /// <summary>Filter by branch. Null = all branches.</summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Filter to a specific account and all its descendants.
    /// Null = entire chart of accounts.
    /// </summary>
    public Guid? ParentAccountId { get; set; }

    /// <summary>
    /// When true, return only leaf accounts (posting accounts).
    /// When false, return all levels including summary accounts.
    /// Null = no filter (return all).
    /// </summary>
    public bool? IsLeafOnly { get; set; }

    /// <summary>
    /// Minimum account level to include (1 = root).
    /// Defaults to 1.
    /// </summary>
    public int MinLevel { get; set; } = 1;

    /// <summary>
    /// Maximum account level to include.
    /// Null = no upper limit.
    /// </summary>
    public int? MaxLevel { get; set; }

    /// <summary>
    /// When true, exclude accounts with zero opening AND zero period movement AND zero closing balance.
    /// </summary>
    public bool HideZeroBalance { get; set; } = false;
}

// ─────────────────────────────────────────────────────────────────────────────
// Trial Balance — Row result
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// One row in the Trial Balance report (ميزان المراجعة).
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

    // ── Opening balance (before FromDate) ────────────────────────────────────
    public decimal OpeningDebit { get; set; }
    public decimal OpeningCredit { get; set; }

    // ── Period movement (FromDate .. ToDate) ─────────────────────────────────
    public decimal PeriodDebit { get; set; }
    public decimal PeriodCredit { get; set; }

    // ── Closing balance ───────────────────────────────────────────────────────
    public decimal ClosingDebit { get; set; }
    public decimal ClosingCredit { get; set; }
}

// ─────────────────────────────────────────────────────────────────────────────
// Trial Balance — Report envelope
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Full Trial Balance report result.
/// </summary>
public class TrialBalanceReportDto
{
    public IReadOnlyList<TrialBalanceRowDto> Rows { get; set; } = [];

    // ── Totals ────────────────────────────────────────────────────────────────
    public decimal TotalOpeningDebit { get; set; }
    public decimal TotalOpeningCredit { get; set; }
    public decimal TotalPeriodDebit { get; set; }
    public decimal TotalPeriodCredit { get; set; }
    public decimal TotalClosingDebit { get; set; }
    public decimal TotalClosingCredit { get; set; }

    // ── Meta ──────────────────────────────────────────────────────────────────
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public Guid? BranchId { get; set; }
    public int RowCount { get; set; }
}
