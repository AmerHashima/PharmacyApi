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

        // Filter entries before the left join so accounts without movement remain
        // visible with a zero amount, while movement outside the period is ignored.
        var periodDetails =
            from detail in journalEntryDetailRepository.GetQueryable()
            join entry in journalEntryRepository.GetQueryable() on detail.JournalEntryId equals entry.Oid
            where !detail.IsDeleted
                  && !entry.IsDeleted
                  && !entry.IsReversed
                  && entry.EntryDate >= fromDate
                  && entry.EntryDate < toDateExclusive
            select new { detail.AccountId, detail.Debit, detail.Credit };

        // Aggregate inside the group join. Projecting a non-nullable decimal from
        // a DefaultIfEmpty() left join causes SQL NULL values for accounts with no
        // period activity, which EF cannot materialize into decimal.
        var accountLines = await (
            from account in accountRepository.GetQueryable()
            where !account.IsDeleted
                  && account.IsActive
                  && (account.AccountCode.StartsWith("4")
                      || account.AccountCode.StartsWith("5")
                      || account.AccountCode.StartsWith("8"))
            join detail in periodDetails on account.Oid equals detail.AccountId into details
            select new
            {
                AccountId = account.Oid,
                account.AccountCode,
                account.AccountNameAr,
                account.AccountNameEn,
                account.AccountLevel,
                Debit = details.Sum(x => (decimal?)x.Debit) ?? 0m,
                Credit = details.Sum(x => (decimal?)x.Credit) ?? 0m
            })
            .ToListAsync(cancellationToken);

        var rows = accountLines
            .Select(account =>
            {
                var isRevenue = account.AccountCode.StartsWith("4", StringComparison.Ordinal);
                var amount = isRevenue
                    ? account.Credit - account.Debit
                    : account.Debit - account.Credit;

                var (sectionNo, sectionNameAr, sectionNameEn) = GetSection(account.AccountCode);
                return new IncomeStatementRowDto
                {
                    SectionNo = sectionNo,
                    SectionNameAr = sectionNameAr,
                    SectionNameEn = sectionNameEn,
                    AccountId = account.AccountId,
                    AccountCode = account.AccountCode,
                    AccountNameAr = account.AccountNameAr,
                    AccountNameEn = account.AccountNameEn,
                    AccountLevel = account.AccountLevel,
                    SortOrder = account.AccountCode,
                    Amount = amount,
                    DisplayAmount = amount,
                    IsBold = account.AccountLevel <= 2,
                    ForeColor = GetForeColor(account.AccountCode),
                    BackColor = account.AccountLevel <= 2 ? "#EBF2FC" : "#FFFFFF"
                };
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
}
