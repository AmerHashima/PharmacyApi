using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Interfaces;
using Pharmacy.Infrastructure.Persistence;

namespace Pharmacy.Infrastructure.Services;

/// <summary>
/// Generates voucher numbers in the format: {YEAR}-{BranchCode}-{Type}-{Sequence:D7}
/// e.g. 2025-BR01-RV-0000001
///
/// Sequences are stored in <see cref="VoucherSequence"/> — one row per
/// (BranchId, VoucherType, Year). The UPDATE … OUTPUT is atomic so concurrent
/// requests never produce the same number.
/// </summary>
public sealed class VoucherNumberService : IVoucherNumberService
{
    private readonly PharmacyDbContext _context;

    public VoucherNumberService(PharmacyDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateAsync(
        Guid branchId,
        string voucherType,
        CancellationToken cancellationToken = default)
    {
        int year = DateTime.UtcNow.Year;

        // Resolve branch code
        var branch = await _context.Branches
            .Where(b => b.Oid == branchId && !b.IsDeleted)
            .Select(b => new { b.BranchCode })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Branch '{branchId}' not found.");

        // Upsert + atomic increment in one statement
        var results = await _context.Database
            .SqlQuery<VoucherSequenceResult>(
                $"""
                MERGE INTO Accounting.VoucherSequences WITH (HOLDLOCK) AS target
                USING (SELECT {branchId} AS BranchId, {voucherType} AS VoucherType, {year} AS [Year]) AS src
                    ON target.BranchId    = src.BranchId
                   AND target.VoucherType = src.VoucherType
                   AND target.[Year]      = src.[Year]
                WHEN MATCHED THEN
                    UPDATE SET LastSequence = target.LastSequence + 1
                WHEN NOT MATCHED THEN
                    INSERT (Oid, BranchId, VoucherType, [Year], LastSequence)
                    VALUES (NEWID(), {branchId}, {voucherType}, {year}, 1)
                OUTPUT INSERTED.LastSequence;
                """)
            .ToListAsync(cancellationToken);

        int seq = results.First().LastSequence;

        return $"{year}-{branch.BranchCode}-{voucherType}-{seq:D7}";
    }

    private sealed record VoucherSequenceResult(int LastSequence);
}
