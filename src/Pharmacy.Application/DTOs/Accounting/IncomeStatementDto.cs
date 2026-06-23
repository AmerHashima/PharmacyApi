namespace Pharmacy.Application.DTOs.Accounting;

/// <summary>Parameters for the income statement (Profit and Loss) report.</summary>
public class IncomeStatementRequest
{
    /// <summary>Start of the reporting period, inclusive.</summary>
    public DateTime FromDate { get; set; }

    /// <summary>End of the reporting period, inclusive.</summary>
    public DateTime ToDate { get; set; }
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
    public string AccountCode { get; set; } = string.Empty;
    public string AccountNameAr { get; set; } = string.Empty;
    public string? AccountNameEn { get; set; }
    public int AccountLevel { get; set; }
    public string SortOrder { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal DisplayAmount { get; set; }
    public bool IsTotal { get; set; }
    public bool IsBold { get; set; }
    public string ForeColor { get; set; } = string.Empty;
    public string BackColor { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string PeriodText { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; }
    public decimal TotalCostOfSales { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal TotalOtherIncomeExpense { get; set; }
    public decimal GrossProfit { get; set; }
    public decimal NetProfit { get; set; }
}
