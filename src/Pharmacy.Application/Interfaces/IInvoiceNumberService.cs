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

    // ── Well-known seed GUIDs for INVOICE_TYPE lookup ────────────────────
    public static readonly Guid LookupMasterInvoiceTypeId         = new("11111111-1111-1111-1111-000000000010");
    public static readonly Guid LookupDetailPosInvoiceId          = new("22222222-2222-2222-2222-000000000001");
    public static readonly Guid LookupDetailReturnPosInvoiceId    = new("22222222-2222-2222-2222-000000000002");
    public static readonly Guid LookupDetailSupplierInvoiceId     = new("22222222-2222-2222-2222-000000000003");
    public static readonly Guid LookupDetailReturnSupplierInvoiceId = new("22222222-2222-2222-2222-000000000004");

    /// <summary>
    /// Atomically increments the NumberValue for the matching InvoiceSetup row
    /// (branch-specific if available, global template otherwise) and returns the
    /// formatted invoice number: e.g. <c>PosInv-0000001</c>.
    /// </summary>
    /// <param name="branchId">The branch requesting the number.</param>
    /// <param name="invoiceTypeId">The AppLookupDetail ID that classifies the invoice type (use the lookup constants above).</param>
    Task<string> GenerateNextAsync(Guid branchId, Guid invoiceTypeId, CancellationToken cancellationToken = default);
}
