using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

/// <summary>
/// Trial Balance (ميزان المراجعة) — pure EF Core LINQ, no raw SQL.
///
///   Step 1 — Load full chart of accounts (flat list, no navigation).
///   Step 2 — Build descendant sets: for each account, collect all descendant IDs
///             (self + children recursively) via iterative stack traversal.
///   Step 3 — Load opening (before FromDate) and period (FromDate..ToDate) journal
///             detail lines; group by leaf AccountId; roll up totals to every ancestor.
///   Step 4 — Build dot-separated TreePath per account for sorting and display.
///   Step 5 — Apply in-memory filters (subtree, IsLeaf, Level, HideZeroBalance).
///   Step 6 — Project TrialBalanceRowDto, compute totals, return envelope.
/// </summary>
public class GetTrialBalanceHandler : IRequestHandler<GetTrialBalanceQuery, TrialBalanceReportDto>
{
    private readonly IAccountRepository _accountRepo;
    private readonly IJournalEntryDetailRepository _detailRepo;
    private readonly IJournalEntryRepository _journalRepo;

    public GetTrialBalanceHandler(
        IAccountRepository accountRepo,
        IJournalEntryDetailRepository detailRepo,
        IJournalEntryRepository journalRepo)
    {
        _accountRepo = accountRepo;
        _detailRepo  = detailRepo;
        _journalRepo = journalRepo;
    }

    public async Task<TrialBalanceReportDto> Handle(
        GetTrialBalanceQuery request,
        CancellationToken cancellationToken)
    {
        var req      = request.Request;
        var fromDate = req.FromDate.Date;
        var toDate   = req.ToDate.Date.AddDays(1); // exclusive upper bound

        // ── Step 1: chart of accounts ─────────────────────────────────────────
        var accounts = await _accountRepo.GetQueryable()
            .AsNoTracking()
            .Where(a => !a.IsDeleted)
            .Select(a => new AccountFlat
            {
                Oid           = a.Oid,
                ParentId      = a.ParentId,
                AccountCode   = a.AccountCode,
                AccountNameAr = a.AccountNameAr,
                AccountNameEn = a.AccountNameEn,
                AccountLevel  = a.AccountLevel,
                IsLeaf        = a.IsLeaf,
            })
            .ToListAsync(cancellationToken);

        var accountMap     = accounts.ToDictionary(a => a.Oid);
        var childrenLookup = accounts
            .Where(a => a.ParentId.HasValue)
            .ToLookup(a => a.ParentId!.Value, a => a.Oid);

        // ── Step 2: descendant sets ───────────────────────────────────────────
        var descendantsOf = new Dictionary<Guid, HashSet<Guid>>(accounts.Count);
        foreach (var acc in accounts)
            descendantsOf[acc.Oid] = BuildDescendants(acc.Oid, childrenLookup);

        // ── Step 3: journal movements ─────────────────────────────────────────
        var journalQuery = _journalRepo.GetQueryable()
            .AsNoTracking()
            .Where(j => !j.IsDeleted && !j.IsReversed);

        if (req.BranchId.HasValue)
            journalQuery = journalQuery.Where(j => j.BranchId == req.BranchId);

        var detailQuery = _detailRepo.GetQueryable()
            .AsNoTracking()
            .Where(d => !d.IsDeleted);

        var openingLines = await (
            from j in journalQuery.Where(j => j.EntryDate < fromDate)
            join d in detailQuery on j.Oid equals d.JournalEntryId
            select new { d.AccountId, d.Debit, d.Credit }
        ).ToListAsync(cancellationToken);

        var periodLines = await (
            from j in journalQuery.Where(j => j.EntryDate >= fromDate && j.EntryDate < toDate)
            join d in detailQuery on j.Oid equals d.JournalEntryId
            select new { d.AccountId, d.Debit, d.Credit }
        ).ToListAsync(cancellationToken);

        // Group by leaf AccountId
        var openingByLeaf = openingLines
            .GroupBy(l => l.AccountId)
            .ToDictionary(g => g.Key, g => (Debit: g.Sum(l => l.Debit), Credit: g.Sum(l => l.Credit)));

        var periodByLeaf = periodLines
            .GroupBy(l => l.AccountId)
            .ToDictionary(g => g.Key, g => (Debit: g.Sum(l => l.Debit), Credit: g.Sum(l => l.Credit)));

        // Roll up to every ancestor
        var openingRolled = new Dictionary<Guid, (decimal Debit, decimal Credit)>(accounts.Count);
        var periodRolled  = new Dictionary<Guid, (decimal Debit, decimal Credit)>(accounts.Count);

        foreach (var acc in accounts)
        {
            decimal oDr = 0, oCr = 0, pDr = 0, pCr = 0;
            foreach (var childId in descendantsOf[acc.Oid])
            {
                if (openingByLeaf.TryGetValue(childId, out var o)) { oDr += o.Debit; oCr += o.Credit; }
                if (periodByLeaf.TryGetValue(childId,  out var p)) { pDr += p.Debit; pCr += p.Credit; }
            }
            openingRolled[acc.Oid] = (oDr, oCr);
            periodRolled[acc.Oid]  = (pDr, pCr);
        }

        // ── Step 4: tree paths ────────────────────────────────────────────────
        var treePaths = accounts.ToDictionary(
            a => a.Oid,
            a => BuildTreePath(a.Oid, accountMap));

        // ── Step 5: filter ────────────────────────────────────────────────────
        IEnumerable<AccountFlat> filtered = accounts;

        if (req.ParentAccountId.HasValue && descendantsOf.TryGetValue(req.ParentAccountId.Value, out var subtree))
            filtered = filtered.Where(a => subtree.Contains(a.Oid));

        if (req.IsLeafOnly.HasValue)
            filtered = filtered.Where(a => a.IsLeaf == req.IsLeafOnly.Value);

        filtered = filtered.Where(a => a.AccountLevel >= req.MinLevel);

        if (req.MaxLevel.HasValue)
            filtered = filtered.Where(a => a.AccountLevel <= req.MaxLevel.Value);

        // ── Step 6: project rows ──────────────────────────────────────────────
        var rows = filtered.Select(acc =>
        {
            var (oDr, oCr) = openingRolled[acc.Oid];
            var (pDr, pCr) = periodRolled[acc.Oid];

            var openingDebit  = oDr > oCr ? oDr - oCr : 0m;
            var openingCredit = oCr > oDr ? oCr - oDr : 0m;

            var closingNet    = (oDr - oCr) + (pDr - pCr);
            var closingDebit  = closingNet > 0 ?  closingNet : 0m;
            var closingCredit = closingNet < 0 ? -closingNet : 0m;

            var parent = acc.ParentId.HasValue && accountMap.TryGetValue(acc.ParentId.Value, out var p) ? p : null;

            return new TrialBalanceRowDto
            {
                Oid           = acc.Oid,
                ParentId      = acc.ParentId,
                ParentCode    = parent?.AccountCode,
                ParentNameAr  = parent?.AccountNameAr,
                ParentNameEn  = parent?.AccountNameEn,
                AccountCode   = acc.AccountCode,
                AccountNameAr = acc.AccountNameAr,
                AccountNameEn = acc.AccountNameEn,
                AccountLevel  = acc.AccountLevel,
                IsLeaf        = acc.IsLeaf,
                DisplayName   = new string(' ', (acc.AccountLevel - 1) * 4) + acc.AccountNameAr,
                TreePath      = treePaths[acc.Oid],
                OpeningDebit  = openingDebit,
                OpeningCredit = openingCredit,
                PeriodDebit   = pDr,
                PeriodCredit  = pCr,
                ClosingDebit  = closingDebit,
                ClosingCredit = closingCredit,
            };
        }).ToList();

        if (req.HideZeroBalance)
            rows = rows.Where(r =>
                r.OpeningDebit != 0 || r.OpeningCredit != 0 ||
                r.PeriodDebit  != 0 || r.PeriodCredit  != 0 ||
                r.ClosingDebit != 0 || r.ClosingCredit != 0).ToList();

        rows = rows.OrderBy(r => r.TreePath).ToList();

        return new TrialBalanceReportDto
        {
            Rows               = rows,
            TotalOpeningDebit  = rows.Sum(r => r.OpeningDebit),
            TotalOpeningCredit = rows.Sum(r => r.OpeningCredit),
            TotalPeriodDebit   = rows.Sum(r => r.PeriodDebit),
            TotalPeriodCredit  = rows.Sum(r => r.PeriodCredit),
            TotalClosingDebit  = rows.Sum(r => r.ClosingDebit),
            TotalClosingCredit = rows.Sum(r => r.ClosingCredit),
            FromDate           = req.FromDate,
            ToDate             = req.ToDate,
            BranchId           = req.BranchId,
            RowCount           = rows.Count,
        };
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static HashSet<Guid> BuildDescendants(Guid accountId, ILookup<Guid, Guid> childrenLookup)
    {
        var result = new HashSet<Guid> { accountId };
        var stack  = new Stack<Guid>();
        stack.Push(accountId);
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            foreach (var child in childrenLookup[current])
                if (result.Add(child))
                    stack.Push(child);
        }
        return result;
    }

    private static string BuildTreePath(Guid accountId, Dictionary<Guid, AccountFlat> map)
    {
        var parts   = new List<string>();
        var current = accountId;
        while (map.TryGetValue(current, out var acc))
        {
            parts.Add(acc.AccountCode);
            if (acc.ParentId.HasValue) current = acc.ParentId.Value;
            else break;
        }
        parts.Reverse();
        return string.Join(".", parts);
    }

    private class AccountFlat
    {
        public Guid    Oid           { get; set; }
        public Guid?   ParentId      { get; set; }
        public string  AccountCode   { get; set; } = string.Empty;
        public string  AccountNameAr { get; set; } = string.Empty;
        public string? AccountNameEn { get; set; }
        public int     AccountLevel  { get; set; }
        public bool    IsLeaf        { get; set; }
    }
}
