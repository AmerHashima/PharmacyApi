using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetTrialBalanceHandler(
    IAccountRepository accountRepo,
    IJournalEntryRepository journalEntryRepo,
    IJournalEntryDetailRepository detailRepo,
    IBranchRepository branchRepo,
    ICostCenterRepository costCenterRepo)
    : IRequestHandler<GetTrialBalanceQuery, TrialBalanceReportDto>
{
    public async Task<TrialBalanceReportDto> Handle(
        GetTrialBalanceQuery query,
        CancellationToken cancellationToken)
    {
        var req = query.Request;
        var toDateExclusive = req.ToDate.Date.AddDays(1); // < toDateExclusive  ≡  <= ToDate

        // ── 1. Load all accounts ─────────────────────────────────────────────
        var accounts = await accountRepo.GetQueryable()
            .Where(a => a.IsActive)
            .Select(a => new
            {
                a.Oid,
                a.ParentId,
                a.AccountCode,
                a.AccountNameAr,
                a.AccountNameEn,
                a.AccountLevel,
                a.IsLeaf
            })
            .ToListAsync(cancellationToken);

        var accountMap = accounts.ToDictionary(a => a.Oid);

        // ── 2. Build tree paths for sorting (code-based dot path) ─────────────
        string BuildTreePath(Guid id)
        {
            var parts = new Stack<string>();
            var current = id;
            while (accountMap.TryGetValue(current, out var node))
            {
                parts.Push(node.AccountCode);
                if (node.ParentId is null) break;
                current = node.ParentId.Value;
            }
            return string.Join(".", parts);
        }

        var treePaths = accounts.ToDictionary(a => a.Oid, a => BuildTreePath(a.Oid));

        // ── 3. Build descendant set for ParentAccountId subtree filter ────────
        HashSet<Guid>? subtreeIds = null;
        if (req.ParentAccountId.HasValue)
        {
            subtreeIds = [];
            var stack = new Stack<Guid>();
            stack.Push(req.ParentAccountId.Value);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                subtreeIds.Add(current);
                foreach (var child in accounts.Where(a => a.ParentId == current))
                    stack.Push(child.Oid);
            }
        }

        // ── 4. Build flat detail lines joined with JournalEntry ───────────────
        var entriesQ = journalEntryRepo.GetQueryable()
            .Where(e => !e.IsReversed);

        if (req.BranchIds.Count > 0)
            entriesQ = entriesQ.Where(e => e.BranchId != null && req.BranchIds.Contains(e.BranchId.Value));

        var detailsQ = detailRepo.GetQueryable();

        if (req.CostCenterIds.Count > 0)
            detailsQ = detailsQ.Where(d => d.CostCenterId != null && req.CostCenterIds.Contains(d.CostCenterId.Value));

        if (req.AccountIds.Count > 0)
            detailsQ = detailsQ.Where(d => req.AccountIds.Contains(d.AccountId));

        var lines = await (
            from d in detailsQ
            join e in entriesQ on d.JournalEntryId equals e.Oid
            select new
            {
                d.AccountId,
                e.BranchId,
                d.CostCenterId,
                e.EntryDate,
                d.Debit,
                d.Credit
            })
            .ToListAsync(cancellationToken);

        // ── 5. Group and aggregate ────────────────────────────────────────────
        var grouped = lines
            .GroupBy(l => (l.AccountId, l.BranchId, l.CostCenterId))
            .Select(g =>
            {
                // Opening: cumulative net before FromDate → split into DR / CR
                var openingNet = g
                    .Where(l => l.EntryDate.Date < req.FromDate.Date)
                    .Sum(l => l.Debit - l.Credit);

                // Period: raw sums within [FromDate, ToDate]
                var periodLines = g.Where(l =>
                    l.EntryDate.Date >= req.FromDate.Date &&
                    l.EntryDate.Date < toDateExclusive);
                var periodDebit  = periodLines.Sum(l => l.Debit);
                var periodCredit = periodLines.Sum(l => l.Credit);

                // Closing: cumulative net through end of ToDate → split into DR / CR
                var closingNet = g
                    .Where(l => l.EntryDate.Date < toDateExclusive)
                    .Sum(l => l.Debit - l.Credit);

                return new
                {
                    g.Key.AccountId,
                    g.Key.BranchId,
                    g.Key.CostCenterId,
                    OpeningDebit  = openingNet  > 0 ? openingNet  : 0m,
                    OpeningCredit = openingNet  < 0 ? -openingNet : 0m,
                    PeriodDebit   = periodDebit,
                    PeriodCredit  = periodCredit,
                    ClosingDebit  = closingNet  > 0 ? closingNet  : 0m,
                    ClosingCredit = closingNet  < 0 ? -closingNet : 0m,
                };
            })
            .ToList();

        // ── 6. Load branch & cost center name lookups ─────────────────────────
        var branchIds = grouped
            .Where(g => g.BranchId.HasValue)
            .Select(g => g.BranchId!.Value)
            .Distinct()
            .ToHashSet();

        var costCenterIds = grouped
            .Where(g => g.CostCenterId.HasValue)
            .Select(g => g.CostCenterId!.Value)
            .Distinct()
            .ToHashSet();

        var branchNames = branchIds.Count > 0
            ? await branchRepo.GetQueryable()
                .Where(b => branchIds.Contains(b.Oid))
                .Select(b => new { b.Oid, b.BranchName })
                .ToDictionaryAsync(b => b.Oid, b => b.BranchName, cancellationToken)
            : new Dictionary<Guid, string>();

        var costCenterNames = costCenterIds.Count > 0
            ? await costCenterRepo.GetQueryable()
                .Where(c => costCenterIds.Contains(c.Oid))
                .Select(c => new { c.Oid, c.NameAr, c.NameEn })
                .ToDictionaryAsync(c => c.Oid, cancellationToken)
            : [];

        // ── 7. Apply account-level filters & assemble rows ────────────────────
        var rows = new List<TrialBalanceRowDto>();

        foreach (var g in grouped)
        {
            if (!accountMap.TryGetValue(g.AccountId, out var acct)) continue;

            // Specific accounts filter
            if (req.AccountIds.Count > 0 && !req.AccountIds.Contains(g.AccountId)) continue;

            // IsLeafOnly filter
            if (req.IsLeafOnly == true && !acct.IsLeaf) continue;

            // Level range filter
            if (acct.AccountLevel < req.MinLevel) continue;
            if (req.MaxLevel.HasValue && acct.AccountLevel > req.MaxLevel.Value) continue;

            // Subtree filter
            if (subtreeIds is not null && !subtreeIds.Contains(g.AccountId)) continue;

            // RemoveZeroBalance filter
            if (req.RemoveZeroBalance &&
                g.OpeningDebit  == 0 && g.OpeningCredit  == 0 &&
                g.PeriodDebit   == 0 && g.PeriodCredit   == 0 &&
                g.ClosingDebit  == 0 && g.ClosingCredit  == 0)
                continue;

            // Resolve parent names
            var parentNameAr = acct.ParentId.HasValue && accountMap.TryGetValue(acct.ParentId.Value, out var parent)
                ? parent.AccountNameAr : null;
            var parentCode = acct.ParentId.HasValue && accountMap.TryGetValue(acct.ParentId.Value, out var parentC)
                ? parentC.AccountCode : null;
            var parentNameEn = acct.ParentId.HasValue && accountMap.TryGetValue(acct.ParentId.Value, out var parentE)
                ? parentE.AccountNameEn : null;

            // Branch names (Branch entity has a single BranchName field)
            string? branchNameAr = g.BranchId.HasValue && branchNames.TryGetValue(g.BranchId.Value, out var bn) ? bn : null;

            // Cost center names
            string? ccNameAr = null, ccNameEn = null;
            if (g.CostCenterId.HasValue && costCenterNames.TryGetValue(g.CostCenterId.Value, out var cc))
            {
                ccNameAr = cc.NameAr;
                ccNameEn = cc.NameEn;
            }

            // DisplayName — indent by level
            var indent = new string(' ', (acct.AccountLevel - 1) * 4);
            var displayName = $"{indent}{acct.AccountNameAr}";

            rows.Add(new TrialBalanceRowDto
            {
                Oid             = g.AccountId,
                ParentId        = acct.ParentId,
                ParentCode      = parentCode,
                ParentNameAr    = parentNameAr,
                ParentNameEn    = parentNameEn,
                AccountCode     = acct.AccountCode,
                AccountNameAr   = acct.AccountNameAr,
                AccountNameEn   = acct.AccountNameEn,
                AccountLevel    = acct.AccountLevel,
                IsLeaf          = acct.IsLeaf,
                DisplayName     = displayName,
                TreePath        = treePaths.TryGetValue(g.AccountId, out var tp) ? tp : acct.AccountCode,
                BranchId        = g.BranchId,
                BranchNameAr    = branchNameAr,
                BranchNameEn    = branchNameAr,   // Branch has a single name field
                CostCenterId    = g.CostCenterId,
                CostCenterNameAr = ccNameAr,
                CostCenterNameEn = ccNameEn,
                OpeningDebit    = g.OpeningDebit,
                OpeningCredit   = g.OpeningCredit,
                PeriodDebit     = g.PeriodDebit,
                PeriodCredit    = g.PeriodCredit,
                ClosingDebit    = g.ClosingDebit,
                ClosingCredit   = g.ClosingCredit,
            });
        }

        // Sort by tree path for a natural account tree order
        rows = [.. rows.OrderBy(r => r.TreePath)
                       .ThenBy(r => r.BranchId.ToString())
                       .ThenBy(r => r.CostCenterId.ToString())];

        // ── 8. Build envelope ─────────────────────────────────────────────────
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
            BranchIds          = req.BranchIds,
            CostCenterIds      = req.CostCenterIds,
            AccountIds         = req.AccountIds,
            RowCount           = rows.Count,
        };
    }
}
