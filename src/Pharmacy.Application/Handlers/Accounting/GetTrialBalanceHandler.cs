using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetTrialBalanceHandler(ITrialBalanceViewRepository viewRepo, IAccountRepository accountRepo)
    : IRequestHandler<GetTrialBalanceQuery, TrialBalanceReportDto>
{
    public async Task<TrialBalanceReportDto> Handle(
        GetTrialBalanceQuery query,
        CancellationToken cancellationToken)
    {
        var req = query.Request;
        var fromDate        = req.FromDate.Date;
        var toDateExclusive = req.ToDate.Date.AddDays(1);

        // ── 1. Base query from view ───────────────────────────────────────────
        var q = viewRepo.GetQueryable();

        // Branch filter
        if (req.BranchIds.Count > 0)
            q = q.Where(r => r.BranchId != null && req.BranchIds.Contains(r.BranchId.Value));

        // Cost center filter
        if (req.CostCenterIds.Count > 0)
            q = q.Where(r => r.CostCenterId != null && req.CostCenterIds.Contains(r.CostCenterId.Value));

        // Specific accounts filter
        if (req.AccountIds.Count > 0)
            q = q.Where(r => req.AccountIds.Contains(r.Oid));

        // IsLeafOnly filter
        if (req.IsLeafOnly == true)
        {
            q = q.Where(r =>
                !accountRepo.GetQueryable().Any(child =>
                    child.ParentId == r.Oid &&
                    !child.IsDeleted));
        }
        else if (req.IsLeafOnly == false)
        {
            q = q.Where(r =>
                accountRepo.GetQueryable().Any(child =>
                    child.ParentId == r.Oid &&
                    !child.IsDeleted));
        }

        // Account level range filter
        if (req.MinLevel > 1)
            q = q.Where(r => r.AccountLevel >= req.MinLevel);
        if (req.MaxLevel.HasValue)
            q = q.Where(r => r.AccountLevel <= req.MaxLevel.Value);

        // ── 2. Group and aggregate on the server ──────────────────────────────
        var grouped = await q
            .GroupBy(r => new
            {
                r.Oid,
                r.ParentId,
                r.ParentCode,
                r.ParentNameAr,
                r.AccountCode,
                r.AccountNameAr,
                r.AccountNameEn,
                r.AccountLevel,
                r.IsLeaf,
                r.DisplayName,
                r.TreePath,
                r.BranchId,
                r.BranchNameAr,
                r.BranchNameEn,
                r.CostCenterId,
                r.CostCenterNameAr,
                r.CostCenterNameEn,
            })
            .Select(g => new
            {
                g.Key.Oid,
                g.Key.ParentId,
                g.Key.ParentCode,
                g.Key.ParentNameAr,
                g.Key.AccountCode,
                g.Key.AccountNameAr,
                g.Key.AccountNameEn,
                g.Key.AccountLevel,
                g.Key.IsLeaf,
                g.Key.DisplayName,
                g.Key.TreePath,
                g.Key.BranchId,
                g.Key.BranchNameAr,
                g.Key.BranchNameEn,
                g.Key.CostCenterId,
                g.Key.CostCenterNameAr,
                g.Key.CostCenterNameEn,

                // Opening: net before FromDate
                OpeningNet = g.Sum(r => r.EntryDate != null && r.EntryDate.Value < fromDate
                    ? r.Debit - r.Credit : 0m),

                // Period: raw sums within [FromDate, ToDate]
                PeriodDebit = g.Sum(r => r.EntryDate != null
                    && r.EntryDate.Value >= fromDate
                    && r.EntryDate.Value < toDateExclusive
                    ? r.Debit : 0m),

                PeriodCredit = g.Sum(r => r.EntryDate != null
                    && r.EntryDate.Value >= fromDate
                    && r.EntryDate.Value < toDateExclusive
                    ? r.Credit : 0m),

                // Closing: cumulative net through end of ToDate
                ClosingNet = g.Sum(r => r.EntryDate != null && r.EntryDate.Value < toDateExclusive
                    ? r.Debit - r.Credit : 0m),
            })
            .ToListAsync(cancellationToken);

        // ── 3. ParentAccountIds subtree filter (in-memory, needs tree walk) ───
        if (req.ParentAccountIds.Count > 0)
        {
            // Collect all account Oids present in the result for tree traversal
            var accountParents = grouped
                .Select(g => (g.Oid, g.ParentId))
                .Distinct()
                .ToDictionary(x => x.Oid, x => x.ParentId);

            var subtreeIds = new HashSet<Guid>(req.ParentAccountIds);
            bool added;
            do
            {
                added = false;
                foreach (var (oid, parentId) in accountParents)
                {
                    if (!subtreeIds.Contains(oid) && parentId.HasValue && subtreeIds.Contains(parentId.Value))
                    {
                        subtreeIds.Add(oid);
                        added = true;
                    }
                }
            } while (added);

            grouped = grouped.Where(g => subtreeIds.Contains(g.Oid)).ToList();
        }

        // ── 4. Assemble rows & apply RemoveZeroBalance ────────────────────────
        var rows = new List<TrialBalanceRowDto>(grouped.Count);

        foreach (var g in grouped)
        {
            var openingDebit  = g.OpeningNet  > 0 ? g.OpeningNet  : 0m;
            var openingCredit = g.OpeningNet  < 0 ? -g.OpeningNet : 0m;
            var closingDebit  = g.ClosingNet  > 0 ? g.ClosingNet  : 0m;
            var closingCredit = g.ClosingNet  < 0 ? -g.ClosingNet : 0m;

            if (req.RemoveZeroBalance &&
                openingDebit  == 0 && openingCredit  == 0 &&
                g.PeriodDebit == 0 && g.PeriodCredit == 0 &&
                closingDebit  == 0 && closingCredit  == 0)
                continue;

            rows.Add(new TrialBalanceRowDto
            {
                Oid              = g.Oid,
                ParentId         = g.ParentId,
                ParentCode       = g.ParentCode,
                ParentNameAr     = g.ParentNameAr,
                AccountCode      = g.AccountCode,
                AccountNameAr    = g.AccountNameAr,
                AccountNameEn    = g.AccountNameEn,
                AccountLevel     = g.AccountLevel,
                IsLeaf           = g.IsLeaf,
                DisplayName      = g.DisplayName,
                TreePath         = g.TreePath,
                BranchId         = g.BranchId,
                BranchNameAr     = g.BranchNameAr,
                BranchNameEn     = g.BranchNameEn,
                CostCenterId     = g.CostCenterId,
                CostCenterNameAr = g.CostCenterNameAr,
                CostCenterNameEn = g.CostCenterNameEn,
                OpeningDebit     = openingDebit,
                OpeningCredit    = openingCredit,
                PeriodDebit      = g.PeriodDebit,
                PeriodCredit     = g.PeriodCredit,
                ClosingDebit     = closingDebit,
                ClosingCredit    = closingCredit,
            });
        }

        rows.Sort((a, b) => string.Compare(a.TreePath, b.TreePath, StringComparison.Ordinal));

        // ── 5. Return envelope ────────────────────────────────────────────────
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
