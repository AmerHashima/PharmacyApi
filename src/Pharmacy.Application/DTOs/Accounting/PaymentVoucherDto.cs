namespace Pharmacy.Application.DTOs.Accounting;

public class PaymentVoucherDto
{
    public Guid Oid { get; set; }
    public string? VoucherNumber { get; set; }
    public DateTime VoucherDate { get; set; }
    public Guid? StakeholderId { get; set; }
    public string? StakeholderName { get; set; }
    public Guid? CashBoxId { get; set; }
    public string? CashBoxName { get; set; }
    public Guid? BankAccountId { get; set; }
    public string? BankAccountName { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
    public Guid? JournalEntryId { get; set; }
    public string? JournalEntryNumber { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class CreatePaymentVoucherDto
{
    public string? VoucherNumber { get; set; }
    public DateTime VoucherDate { get; set; }
    public Guid? StakeholderId { get; set; }
    public Guid? CashBoxId { get; set; }
    public Guid? BankAccountId { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
    public Guid? JournalEntryId { get; set; }
}

public class UpdatePaymentVoucherDto
{
    public Guid Oid { get; set; }
    public string? VoucherNumber { get; set; }
    public DateTime VoucherDate { get; set; }
    public Guid? StakeholderId { get; set; }
    public Guid? CashBoxId { get; set; }
    public Guid? BankAccountId { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
    public Guid? JournalEntryId { get; set; }
}
