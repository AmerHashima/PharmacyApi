namespace Pharmacy.Application.DTOs.Accounting;

/// <summary>Parameters for the income statement (Profit and Loss) report.</summary>
public class IncomeStatementRequest
{
    /// <summary>Start of the reporting period, inclusive.</summary>
    public DateTime FromDate { get; set; }

    /// <summary>End of the reporting period, inclusive.</summary>
    public DateTime ToDate { get; set; }

    /// <summary>Optional branch filter. Empty means all branches.</summary>
    public List<Guid> BranchIds { get; set; } = [];

    /// <summary>Optional cost-center filter. Empty means all cost centers.</summary>
    public List<Guid> CostCenterIds { get; set; } = [];

    /// <summary>true for posting accounts, false for summary accounts, null for all.</summary>
    public bool? IsLeafOnly { get; set; }
}

/// <summary>
/// One account line in the income statement. Report totals are repeated on every
/// row to match the result shape required by report viewers/exporters.
/// </summary>
public class IncomeStatementRowDto
{
    public int SectionNo { get; set; }
    public string SectionNameAr { get; set; } = string.Empty;
    public string SectionNameEn { get; set; } = string.Empty;
    public string LineType { get; set; } = "ACCOUNT";
    public Guid AccountId { get; set; }
    public Guid? ParentId { get; set; }
    public string? ParentCode { get; set; }
    public string? ParentNameAr { get; set; }
    public string? ParentNameEn { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountNameAr { get; set; } = string.Empty;
    public string? AccountNameEn { get; set; }
    public int AccountLevel { get; set; }
    public bool IsLeaf { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string TreePath { get; set; } = string.Empty;
    public string SortOrder { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal Amount { get; set; }
    public decimal DisplayAmount { get; set; }
    public bool IsTotal { get; set; }
    public bool IsBold { get; set; }
    public string ForeColor { get; set; } = string.Empty;
    public string BackColor { get; set; } = string.Empty;
    public Guid? BranchId { get; set; }
    public string? BranchNameAr { get; set; }
    public string? BranchNameEn { get; set; }
    public Guid? CostCenterId { get; set; }
    public string? CostCenterNameAr { get; set; }
    public string? CostCenterNameEn { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string PeriodText { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; }
    public decimal TotalCostOfSales { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal TotalOtherIncomeExpense { get; set; }
    public decimal GrossProfit { get; set; }
    public decimal NetProfit { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public decimal RevenueDebit { get; set; }
    public decimal RevenueCredit { get; set; }
    public decimal CostDebit { get; set; }
    public decimal CostCredit { get; set; }
    public decimal ExpensesDebit { get; set; }
    public decimal ExpensesCredit { get; set; }
    public decimal OtherDebit { get; set; }
    public decimal OtherCredit { get; set; }
}

public class BalanceSheetRequest
{
    public DateTime AsOfDate { get; set; }
}

public class BalanceSheetRowDto
{
    public int SectionNo { get; set; }
    public string SectionNameAr { get; set; } = string.Empty;
    public string SectionNameEn { get; set; } = string.Empty;
    public Guid AccountId { get; set; }
    public Guid? ParentId { get; set; }
    public string? ParentCode { get; set; }
    public string? ParentNameAr { get; set; }
    public string? ParentNameEn { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountNameAr { get; set; } = string.Empty;
    public string? AccountNameEn { get; set; }
    public int AccountLevel { get; set; }
    public bool IsLeaf { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string TreePath { get; set; } = string.Empty;
    public string SortOrder { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal DisplayAmount { get; set; }
    public bool IsTotal { get; set; }
    public bool IsBold { get; set; }
    public string ForeColor { get; set; } = string.Empty;
    public string BackColor { get; set; } = string.Empty;
    public DateTime AsOfDate { get; set; }
    public string AsOfDateText { get; set; } = string.Empty;
    public decimal TotalAssets { get; set; }
    public decimal TotalLiabilities { get; set; }
    public decimal TotalEquity { get; set; }
    public decimal TotalLiabilitiesAndEquity { get; set; }
    public string BalanceStatus { get; set; } = string.Empty;
}

public class BalanceSheetDebitCreditRowDto : BalanceSheetRowDto
{
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
}
