using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Interfaces;
using Pharmacy.Infrastructure.Persistence;
using System.Data.Common;
using Pharmacy.Domain.Interfaces.Accounting;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.Infrastructure.Services;

public class AccountingReportService : IAccountingReportService
{
    private readonly PharmacyDbContext _context;
    private readonly IAccountRepository _accountRepository;

    public AccountingReportService(PharmacyDbContext context, IAccountRepository accountRepository)
    {
        _context = context;
        _accountRepository = accountRepository;
    }

    public async Task<IReadOnlyList<IncomeStatementRowDto>> GetIncomeStatementAsync(IncomeStatementRequest request, CancellationToken cancellationToken = default)
    {
        var fromDate = request.FromDate.Date;
        var toDate = request.ToDate.Date;

        if (toDate < fromDate)
            throw new ArgumentException("ToDate must be on or after FromDate.");

        var sql = @"
SELECT
    R.SectionNo,
    R.SectionNameAr,
    R.SectionNameEn,
    R.LineType,
    R.AccountId,
    R.AccountCode,
    R.AccountNameAr,
    R.AccountNameEn,
    R.AccountLevel,
    R.SortOrder,
    R.Amount,
    R.DisplayAmount,
    R.IsTotal,
    R.IsBold,
    R.ForeColor,
    R.BackColor,
    CAST(@FromDate AS DATE) AS FromDate,
    CAST(@ToDate AS DATE) AS ToDate,
    CONVERT(NVARCHAR(10), @FromDate, 103) + N' - ' + CONVERT(NVARCHAR(10), @ToDate, 103) AS PeriodText,

    SUM(CASE WHEN R.SectionNo = 1 AND R.IsTotal = 0 THEN R.Amount ELSE 0 END) OVER() AS TotalRevenue,
    SUM(CASE WHEN R.SectionNo = 2 AND R.IsTotal = 0 THEN R.Amount ELSE 0 END) OVER() AS TotalCostOfSales,
    SUM(CASE WHEN R.SectionNo = 4 AND R.IsTotal = 0 THEN R.Amount ELSE 0 END) OVER() AS TotalExpenses,
    SUM(CASE WHEN R.SectionNo = 5 AND R.IsTotal = 0 THEN R.Amount ELSE 0 END) OVER() AS TotalOtherIncomeExpense,

    (
        SUM(CASE WHEN R.SectionNo = 1 AND R.IsTotal = 0 THEN R.Amount ELSE 0 END) OVER()
        -
        SUM(CASE WHEN R.SectionNo = 2 AND R.IsTotal = 0 THEN R.Amount ELSE 0 END) OVER()
    ) AS GrossProfit,

    (
        SUM(CASE WHEN R.SectionNo = 1 AND R.IsTotal = 0 THEN R.Amount ELSE 0 END) OVER()
        -
        SUM(CASE WHEN R.SectionNo = 2 AND R.IsTotal = 0 THEN R.Amount ELSE 0 END) OVER()
        -
        SUM(CASE WHEN R.SectionNo = 4 AND R.IsTotal = 0 THEN R.Amount ELSE 0 END) OVER()
        +
        SUM(CASE WHEN R.SectionNo = 5 AND R.IsTotal = 0 THEN R.Amount ELSE 0 END) OVER()
    ) AS NetProfit

FROM
(
    SELECT
        CASE
            WHEN A.AccountCode LIKE '4%' THEN 1
            WHEN A.AccountCode LIKE '515%' THEN 2
            WHEN A.AccountCode LIKE '5%' THEN 4
            WHEN A.AccountCode LIKE '8%' THEN 5
            ELSE 9
        END AS SectionNo,

        CASE
            WHEN A.AccountCode LIKE '4%' THEN N'الإيرادات'
            WHEN A.AccountCode LIKE '515%' THEN N'تكلفة المبيعات'
            WHEN A.AccountCode LIKE '5%' THEN N'المصروفات التشغيلية'
            WHEN A.AccountCode LIKE '8%' THEN N'إيرادات ومصروفات أخرى'
            ELSE N'أخرى'
        END AS SectionNameAr,

        CASE
            WHEN A.AccountCode LIKE '4%' THEN N'Revenue'
            WHEN A.AccountCode LIKE '515%' THEN N'Cost Of Sales'
            WHEN A.AccountCode LIKE '5%' THEN N'Operating Expenses'
            WHEN A.AccountCode LIKE '8%' THEN N'Other Income / Expenses'
            ELSE N'Other'
        END AS SectionNameEn,

        N'ACCOUNT' AS LineType,
        A.Oid AS AccountId,
        A.AccountCode,
        A.AccountNameAr,
        A.AccountNameEn,
        A.AccountLevel,
        A.AccountCode AS SortOrder,

        CASE
            WHEN A.AccountCode LIKE '4%'
                THEN SUM(ISNULL(JD.Credit, 0) - ISNULL(JD.Debit, 0))
            ELSE
                SUM(ISNULL(JD.Debit, 0) - ISNULL(JD.Credit, 0))
        END AS Amount,

        CASE
            WHEN A.AccountCode LIKE '4%'
                THEN SUM(ISNULL(JD.Credit, 0) - ISNULL(JD.Debit, 0))
            ELSE
                SUM(ISNULL(JD.Debit, 0) - ISNULL(JD.Credit, 0))
        END AS DisplayAmount,

        CAST(0 AS BIT) AS IsTotal,
        CAST(CASE WHEN A.AccountLevel <= 2 THEN 1 ELSE 0 END AS BIT) AS IsBold,

        CASE
            WHEN A.AccountCode LIKE '4%' THEN N'#093164'
            WHEN A.AccountCode LIKE '515%' THEN N'#DC2626'
            WHEN A.AccountCode LIKE '5%' THEN N'#DC2626'
            WHEN A.AccountCode LIKE '8%' THEN N'#2E8B57'
            ELSE N'#111827'
        END AS ForeColor,

        CASE
            WHEN A.AccountLevel <= 2 THEN N'#EBF2FC'
            ELSE N'#FFFFFF'
        END AS BackColor

    FROM Accounting.Accounts A
    LEFT JOIN Accounting.JournalEntryDetails JD
        ON JD.AccountId = A.Oid
       AND JD.IsDeleted = 0
    LEFT JOIN Accounting.JournalEntries JE
        ON JE.Oid = JD.JournalEntryId
       AND JE.IsDeleted = 0
       AND JE.IsReversed = 0
       AND JE.EntryDate >= @FromDate
       AND JE.EntryDate < DATEADD(DAY, 1, @ToDate)
    WHERE A.IsDeleted = 0
      AND A.IsActive = 1
      AND (
            A.AccountCode LIKE '4%'
         OR A.AccountCode LIKE '5%'
         OR A.AccountCode LIKE '8%'
      )
";

        var whereExtras = new List<string>();
        var paramPairs = new List<(string Name, object Value)>();

        // Branch filters
        if (request.BranchIds.Count > 0)
        {
            var names = request.BranchIds.Select((b, i) => "@branch" + i).ToList();
            whereExtras.Add($"AND (JE.BranchId IS NOT NULL AND JE.BranchId IN ({string.Join(',', names)}))");
            for (int i = 0; i < request.BranchIds.Count; i++)
                paramPairs.Add((names[i], request.BranchIds[i]));
        }

        // Cost center filters
        if (request.CostCenterIds.Count > 0)
        {
            var names = request.CostCenterIds.Select((c, i) => "@cc" + i).ToList();
            whereExtras.Add($"AND (JD.CostCenterId IS NOT NULL AND JD.CostCenterId IN ({string.Join(',', names)}))");
            for (int i = 0; i < request.CostCenterIds.Count; i++)
                paramPairs.Add((names[i], request.CostCenterIds[i]));
        }

        if (request.IsLeafOnly.HasValue && request.IsLeafOnly.Value)
        {
            whereExtras.Add("AND A.IsLeaf = 1");
        }

        sql += string.Join('\n', whereExtras);

        sql += @"
    GROUP BY
        A.Oid,
        A.AccountCode,
        A.AccountNameAr,
        A.AccountNameEn,
        A.AccountLevel
) R
ORDER BY
    R.SectionNo,
    R.SortOrder;";

        var rows = new List<IncomeStatementRowDto>();

        await using var conn = _context.Database.GetDbConnection();
        await conn.OpenAsync(cancellationToken);
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        var pFrom = cmd.CreateParameter(); pFrom.ParameterName = "@FromDate"; pFrom.Value = fromDate; cmd.Parameters.Add(pFrom);
        var pTo = cmd.CreateParameter(); pTo.ParameterName = "@ToDate"; pTo.Value = toDate; cmd.Parameters.Add(pTo);

        // add dynamic params
        foreach (var (Name, Value) in paramPairs)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = Name;
            p.Value = Value;
            cmd.Parameters.Add(p);
        }

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var row = new IncomeStatementRowDto
            {
                SectionNo = reader.GetInt32(reader.GetOrdinal("SectionNo")),
                SectionNameAr = reader.GetString(reader.GetOrdinal("SectionNameAr")),
                SectionNameEn = reader.GetString(reader.GetOrdinal("SectionNameEn")),
                LineType = reader.GetString(reader.GetOrdinal("LineType")),
                AccountId = reader.GetGuid(reader.GetOrdinal("AccountId")),
                AccountCode = reader.GetString(reader.GetOrdinal("AccountCode")),
                AccountNameAr = reader.GetString(reader.GetOrdinal("AccountNameAr")),
                AccountNameEn = reader.IsDBNull(reader.GetOrdinal("AccountNameEn")) ? null : reader.GetString(reader.GetOrdinal("AccountNameEn")),
                AccountLevel = reader.GetInt32(reader.GetOrdinal("AccountLevel")),
                SortOrder = reader.GetString(reader.GetOrdinal("SortOrder")),
                Amount = reader.IsDBNull(reader.GetOrdinal("Amount")) ? 0m : reader.GetDecimal(reader.GetOrdinal("Amount")),
                DisplayAmount = reader.IsDBNull(reader.GetOrdinal("DisplayAmount")) ? 0m : reader.GetDecimal(reader.GetOrdinal("DisplayAmount")),
                IsTotal = reader.GetBoolean(reader.GetOrdinal("IsTotal")),
                IsBold = reader.GetBoolean(reader.GetOrdinal("IsBold")),
                ForeColor = reader.GetString(reader.GetOrdinal("ForeColor")),
                BackColor = reader.GetString(reader.GetOrdinal("BackColor")),
                FromDate = reader.GetDateTime(reader.GetOrdinal("FromDate")),
                ToDate = reader.GetDateTime(reader.GetOrdinal("ToDate")),
                PeriodText = reader.GetString(reader.GetOrdinal("PeriodText")),
                TotalRevenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue")),
                TotalCostOfSales = reader.GetDecimal(reader.GetOrdinal("TotalCostOfSales")),
                TotalExpenses = reader.GetDecimal(reader.GetOrdinal("TotalExpenses")),
                TotalOtherIncomeExpense = reader.GetDecimal(reader.GetOrdinal("TotalOtherIncomeExpense")),
                GrossProfit = reader.GetDecimal(reader.GetOrdinal("GrossProfit")),
                NetProfit = reader.GetDecimal(reader.GetOrdinal("NetProfit"))
            };

            rows.Add(row);
        }

        // Enrich parent and tree path info from accounts table
        var accountIds = rows.Select(r => r.AccountId).Distinct().ToList();
        if (accountIds.Count > 0)
        {
            var accounts = await _accountRepository.GetQueryable()
                .Where(a => accountIds.Contains(a.Oid))
                .Select(a => new { a.Oid, a.ParentId, a.AccountCode, a.AccountNameAr, a.AccountNameEn, a.AccountLevel, a.IsLeaf })
                .ToListAsync(cancellationToken);

            var accountById = accounts.ToDictionary(a => a.Oid, a => (dynamic)a);
            foreach (var r in rows)
            {
                if (accountById.TryGetValue(r.AccountId, out dynamic acc))
                {
                    r.ParentId = (Guid?)acc.ParentId;
                    r.ParentCode = acc.ParentId.HasValue && accountById.ContainsKey(acc.ParentId.Value) ? accountById[acc.ParentId.Value].AccountCode : null;
                    r.ParentNameAr = acc.ParentId.HasValue && accountById.ContainsKey(acc.ParentId.Value) ? accountById[acc.ParentId.Value].AccountNameAr : null;
                    r.ParentNameEn = acc.ParentId.HasValue && accountById.ContainsKey(acc.ParentId.Value) ? accountById[acc.ParentId.Value].AccountNameEn : null;
                    r.IsLeaf = acc.IsLeaf;
                    r.DisplayName = $"{new string(' ', Math.Max(0, acc.AccountLevel - 1) * 4)}{acc.AccountNameAr}";
                    r.TreePath = BuildTreePath(acc.Oid, accountById);
                }
            }
        }

        return rows;
    }

    private static string BuildTreePath(Guid accountId, IReadOnlyDictionary<Guid, dynamic> accounts)
    {
        var path = new Stack<string>();
        var visited = new HashSet<Guid>();
        Guid? currentId = accountId;

        while (currentId.HasValue && visited.Add(currentId.Value) && accounts.TryGetValue(currentId.Value, out var account))
        {
            path.Push((string)account.AccountCode);
            currentId = account.ParentId;
        }

        return string.Join('.', path);
    }
}
