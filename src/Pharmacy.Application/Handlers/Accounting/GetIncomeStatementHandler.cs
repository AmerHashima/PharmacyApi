using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

/// <summary>
/// Builds the income statement from active income-statement accounts and journal
/// lines in the requested period. Reversed and soft-deleted entries are excluded.
/// </summary>
public class GetIncomeStatementHandler(
    IAccountRepository accountRepository,
    IJournalEntryRepository journalEntryRepository,
    IJournalEntryDetailRepository journalEntryDetailRepository)
    : IRequestHandler<GetIncomeStatementQuery, IReadOnlyList<IncomeStatementRowDto>>
{
    private sealed record AccountNode(
        Guid Oid,
        Guid? ParentId,
        string AccountCode,
        string AccountNameAr,
        string? AccountNameEn,
        int AccountLevel,
        bool IsLeaf);

    private sealed record ReportDimension(
        Guid? BranchId,
        string? BranchName,
        Guid? CostCenterId,
        string? CostCenterNameAr,
        string? CostCenterNameEn,
        decimal Debit,
        decimal Credit);

    public async Task<IReadOnlyList<IncomeStatementRowDto>> Handle(
        GetIncomeStatementQuery query,
        CancellationToken cancellationToken)
    {
        var request = query.Request;
        var fromDate = request.FromDate.Date;
        var toDate = request.ToDate.Date;

        if (toDate < fromDate)
            throw new ArgumentException("ToDate must be on or after FromDate.");

        var toDateExclusive = toDate.AddDays(1);

        // Filter entries by period before aggregation. Reversed and soft-deleted
        // journal entries/details are never included in the report.
        var periodDetails =
            from detail in journalEntryDetailRepository.GetQueryable()
            join entry in journalEntryRepository.GetQueryable() on detail.JournalEntryId equals entry.Oid
            where !detail.IsDeleted
                  && !entry.IsDeleted
                  && !entry.IsReversed
                  && entry.EntryDate >= fromDate
                  && entry.EntryDate < toDateExclusive
            select new
            {
                detail.AccountId,
                detail.Debit,
                detail.Credit,
                entry.BranchId,
                BranchName = entry.Branch == null ? (string?)null : entry.Branch.BranchName,
                detail.CostCenterId,
                CostCenterNameAr = detail.CostCenter == null ? null : detail.CostCenter.NameAr,
                CostCenterNameEn = detail.CostCenter == null ? null : detail.CostCenter.NameEn
            };

        if (request.BranchIds.Count > 0)
            periodDetails = periodDetails.Where(x => x.BranchId != null && request.BranchIds.Contains(x.BranchId.Value));

        if (request.CostCenterIds.Count > 0)
            periodDetails = periodDetails.Where(x => x.CostCenterId != null && request.CostCenterIds.Contains(x.CostCenterId.Value));

        var journalLines = await periodDetails.ToListAsync(cancellationToken);

        // Include all accounts in the node map so parent labels and paths are
        // available even when a parent has no movement in this reporting period.
        var accountNodes = await accountRepository.GetQueryable()
            .Where(a => !a.IsDeleted && a.IsActive)
            .Select(a => new AccountNode(
                a.Oid,
                a.ParentId,
                a.AccountCode,
                a.AccountNameAr,
                a.AccountNameEn,
                a.AccountLevel,
                a.IsLeaf))
            .ToListAsync(cancellationToken);

        var accountById = accountNodes.ToDictionary(a => a.Oid);
        var reportAccounts = accountNodes.Where(a =>
            a.AccountCode.StartsWith("4", StringComparison.Ordinal)
            || a.AccountCode.StartsWith("5", StringComparison.Ordinal)
            || a.AccountCode.StartsWith("8", StringComparison.Ordinal));

        if (request.IsLeafOnly.HasValue)
            reportAccounts = reportAccounts.Where(a => a.IsLeaf == request.IsLeafOnly.Value);

        var rows = reportAccounts
            .SelectMany(account =>
            {
                var dimensions = journalLines
                    .Where(line => line.AccountId == account.Oid)
                    .GroupBy(line => new
                    {
                        line.BranchId,
                        line.BranchName,
                        line.CostCenterId,
                        line.CostCenterNameAr,
                        line.CostCenterNameEn
                    })
                    .Select(group => new ReportDimension(
                        group.Key.BranchId,
                        group.Key.BranchName,
                        group.Key.CostCenterId,
                        group.Key.CostCenterNameAr,
                        group.Key.CostCenterNameEn,
                        group.Sum(x => x.Debit),
                        group.Sum(x => x.Credit)))
                    .ToList();

                // Keep zero-balance accounts visible, just like the supplied SQL.
                if (dimensions.Count == 0)
                    dimensions.Add(new ReportDimension(null, null, null, null, null, 0m, 0m));

                var parent = account.ParentId.HasValue && accountById.TryGetValue(account.ParentId.Value, out var parentAccount)
                    ? parentAccount
                    : null;

                return dimensions.Select(dimension =>
                {
                var isRevenue = account.AccountCode.StartsWith("4", StringComparison.Ordinal);
                var amount = isRevenue
                    ? dimension.Credit - dimension.Debit
                    : dimension.Debit - dimension.Credit;

                var (sectionNo, sectionNameAr, sectionNameEn) = GetSection(account.AccountCode);
                return new IncomeStatementRowDto
                {
                    SectionNo = sectionNo,
                    SectionNameAr = sectionNameAr,
                    SectionNameEn = sectionNameEn,
                    AccountId = account.Oid,
                    ParentId = account.ParentId,
                    ParentCode = parent?.AccountCode,
                    ParentNameAr = parent?.AccountNameAr,
                    ParentNameEn = parent?.AccountNameEn,
                    AccountCode = account.AccountCode,
                    AccountNameAr = account.AccountNameAr,
                    AccountNameEn = account.AccountNameEn,
                    AccountLevel = account.AccountLevel,
                    IsLeaf = account.IsLeaf,
                    DisplayName = $"{new string(' ', Math.Max(0, account.AccountLevel - 1) * 4)}{account.AccountNameAr}",
                    TreePath = GetTreePath(account.Oid, accountById),
                    SortOrder = account.AccountCode,
                    Amount = amount,
                    DisplayAmount = amount,
                    IsBold = account.AccountLevel <= 2,
                    ForeColor = GetForeColor(account.AccountCode),
                    BackColor = account.AccountLevel <= 2 ? "#EBF2FC" : "#FFFFFF",
                    BranchId = dimension.BranchId,
                    BranchNameAr = dimension.BranchName,
                    BranchNameEn = dimension.BranchName,
                    CostCenterId = dimension.CostCenterId,
                    CostCenterNameAr = dimension.CostCenterNameAr,
                    CostCenterNameEn = dimension.CostCenterNameEn
                };
                });
            })
            .OrderBy(x => x.SectionNo)
            .ThenBy(x => x.SortOrder, StringComparer.Ordinal)
            .ToList();

        var totalRevenue = rows.Where(x => x.SectionNo == 1 && !x.IsTotal).Sum(x => x.Amount);
        var totalCostOfSales = rows.Where(x => x.SectionNo == 2 && !x.IsTotal).Sum(x => x.Amount);
        var totalExpenses = rows.Where(x => x.SectionNo == 4 && !x.IsTotal).Sum(x => x.Amount);
        var totalOtherIncomeExpense = rows.Where(x => x.SectionNo == 5 && !x.IsTotal).Sum(x => x.Amount);
        var grossProfit = totalRevenue - totalCostOfSales;
        var netProfit = grossProfit - totalExpenses + totalOtherIncomeExpense;
        var periodText = $"{fromDate:dd/MM/yyyy} - {toDate:dd/MM/yyyy}";

        foreach (var row in rows)
        {
            row.FromDate = fromDate;
            row.ToDate = toDate;
            row.PeriodText = periodText;
            row.TotalRevenue = totalRevenue;
            row.TotalCostOfSales = totalCostOfSales;
            row.TotalExpenses = totalExpenses;
            row.TotalOtherIncomeExpense = totalOtherIncomeExpense;
            row.GrossProfit = grossProfit;
            row.NetProfit = netProfit;
        }

        return rows;
    }

    private static (int Number, string NameAr, string NameEn) GetSection(string accountCode) =>
        accountCode.StartsWith("4", StringComparison.Ordinal) ? (1, "الإيرادات", "Revenue") :
        accountCode.StartsWith("515", StringComparison.Ordinal) ? (2, "تكلفة المبيعات", "Cost Of Sales") :
        accountCode.StartsWith("5", StringComparison.Ordinal) ? (4, "المصروفات التشغيلية", "Operating Expenses") :
        accountCode.StartsWith("8", StringComparison.Ordinal) ? (5, "إيرادات ومصروفات أخرى", "Other Income / Expenses") :
        (9, "أخرى", "Other");

    private static string GetForeColor(string accountCode) =>
        accountCode.StartsWith("4", StringComparison.Ordinal) ? "#093164" :
        accountCode.StartsWith("515", StringComparison.Ordinal) ? "#DC2626" :
        accountCode.StartsWith("5", StringComparison.Ordinal) ? "#DC2626" :
        accountCode.StartsWith("8", StringComparison.Ordinal) ? "#2E8B57" : "#111827";

    private static string GetTreePath(Guid accountId, IReadOnlyDictionary<Guid, AccountNode> accounts)
    {
        var path = new Stack<string>();
        var visited = new HashSet<Guid>();
        Guid? currentId = accountId;

        while (currentId.HasValue && visited.Add(currentId.Value) && accounts.TryGetValue(currentId.Value, out var account))
        {
            path.Push(account.AccountCode);
            currentId = account.ParentId;
        }

        return string.Join('.', path);
    }
}
