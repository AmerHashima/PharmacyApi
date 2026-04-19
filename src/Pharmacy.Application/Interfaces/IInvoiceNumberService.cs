namespace Pharmacy.Application.Interfaces;

/// <summary>
/// Generates unique, sequentially-incrementing invoice numbers from the InvoiceSetup table.
/// The operation is atomic: concurrent calls will never produce the same number.
/// </summary>
public interface IInvoiceNumberService
{
    // ── Standard format codes matching InvoiceSetup seed data ──────────────
    public const string FormatPosInvoice       = "PosInv";
    public const string FormatReturnPosInvoice = "ReturnPosInv";
    public const string FormatSupplierInvoice  = "SupplierInv";
    public const string FormatReturnSupplier   = "ReturnSuppInv";

    /// <summary>
    /// Atomically increments the NumberValue for the matching InvoiceSetup row
    /// (branch-specific if available, global template otherwise) and returns the
    /// formatted invoice number: e.g. <c>PosInv0000001</c>.
    /// </summary>
    /// <param name="branchId">The branch requesting the number.</param>
    /// <param name="format">The format code (use the constants above).</param>
    Task<string> GenerateNextAsync(Guid branchId, string format, CancellationToken cancellationToken = default);
}
