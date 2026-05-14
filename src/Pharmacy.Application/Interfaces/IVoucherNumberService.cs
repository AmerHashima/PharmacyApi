namespace Pharmacy.Application.Interfaces;

/// <summary>
/// Generates unique voucher numbers in the format: {YEAR}-{BranchCode}-{Sequence:D7}
/// e.g. 2025-BR01-0000001
/// The sequence is per voucher-type per branch and is incremented atomically.
/// </summary>
public interface IVoucherNumberService
{
    public const string TypeReceipt = "RV";
    public const string TypePayment = "PV";

    /// <summary>
    /// Atomically generates the next voucher number for the given branch and voucher type.
    /// </summary>
    Task<string> GenerateAsync(Guid branchId, string voucherType, CancellationToken cancellationToken = default);
}
