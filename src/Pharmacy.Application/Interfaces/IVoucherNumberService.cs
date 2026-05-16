namespace Pharmacy.Application.Interfaces;

/// <summary>
/// Generates unique voucher/journal numbers in the format: {YEAR}-{BranchCode}-{Type}-{Sequence:D7}
/// e.g. 2025-BR01-RV-0000001
/// The sequence is per type per branch and is incremented atomically.
/// </summary>
public interface IVoucherNumberService
{
    public const string TypeReceipt      = "RV";
    public const string TypePayment      = "PV";
    public const string TypeJournalEntry = "JE";
    public const string RECEIPT_VOUCHER = "RECEIPT_VOUCHER";
    public const string PAYMENT_VOUCHER = "PAYMENT_VOUCHER";

    /// <summary>
    /// Atomically generates the next voucher number for the given branch and voucher type.
    /// </summary>
    Task<string> GenerateAsync(Guid branchId, string voucherType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atomically generates the next journal entry number (independent sequence from vouchers).
    /// </summary>
    Task<string> GenerateJournalEntryNumberAsync(Guid branchId, CancellationToken cancellationToken = default);
}
