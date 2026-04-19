using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Services;

/// <summary>
/// Generates invoice numbers by atomically incrementing <c>InvoiceSetup.NumberValue</c>
/// in a single SQL <c>UPDATE … OUTPUT</c> statement, guaranteeing uniqueness even under
/// high concurrency without application-level locking.
///
/// Preference order:
///   1. Branch-specific row  (BranchId = <paramref name="branchId"/>)
///   2. Global template row  (BranchId IS NULL)
/// </summary>
public sealed class InvoiceNumberService : IInvoiceNumberService
{
    private readonly PharmacyDbContext _context;

    public InvoiceNumberService(PharmacyDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextAsync(
        Guid branchId,
        string format,
        CancellationToken cancellationToken = default)
    {
        // Single atomic statement:
        //   - Finds the correct row (branch-specific first, global fallback)
        //   - Increments NumberValue in the database
        //   - Returns the new value in one round-trip — no race conditions possible
        var results = await _context.Database
            .SqlQuery<InvoiceNumberResult>(
                $"""
                UPDATE InvoiceSetups
                SET    NumberValue = NumberValue + 1
                OUTPUT INSERTED.Format, INSERTED.NumberValue
                WHERE  Oid = (
                    SELECT TOP 1 Oid
                    FROM   InvoiceSetups
                    WHERE  Format    = {format}
                      AND  IsDeleted = 0
                      AND  (BranchId = {branchId} OR BranchId IS NULL)
                    ORDER BY CASE WHEN BranchId IS NOT NULL THEN 0 ELSE 1 END
                )
                """)
            .ToListAsync(cancellationToken);

        var result = results.FirstOrDefault()
            ?? throw new InvalidOperationException(
                $"No InvoiceSetup row found for format '{format}'. " +
                $"Add a branch-specific row or ensure the global template exists.");

        // Format: e.g.  PosInv0000001
        return $"{result.Format}-{result.NumberValue:D7}";
    }

    // EF Core SqlQuery<T> maps OUTPUT columns by name to this record's properties.
    private sealed record InvoiceNumberResult(string Format, int NumberValue);
}
