namespace Pharmacy.Application.DTOs.Accounting;

public class PaymentVoucherMasterDto
{
    public Guid Oid { get; set; }
    public string? VoucherNumber { get; set; }
    public DateTime VoucherDate { get; set; }
    public Guid? FiscalYearId { get; set; }
    public Guid? BranchId { get; set; }
    public string? BranchName { get; set; }
    public Guid? CashBoxId { get; set; }
    public string? CashBoxName { get; set; }
    public Guid? BankAccountId { get; set; }
    public string? BankAccountName { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    public Guid? JournalEntryId { get; set; }
    public string? JournalEntryNumber { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class PaymentVoucherDetailDto
{
    public Guid Oid { get; set; }
    public Guid PaymentVoucherId { get; set; }
    public Guid AccountId { get; set; }
    public string? AccountCode { get; set; }
    public string? AccountNameAr { get; set; }
    public Guid? CostCenterId { get; set; }
    public string? CostCenterNameAr { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public Guid? StakeholderId { get; set; }
    public string? StakeholderName { get; set; }
    public string? ReferenceInvoiceId { get; set; }
}

public class PaymentVoucherDto
{
    public Guid Oid { get; set; }
    public string? VoucherNumber { get; set; }
    public DateTime VoucherDate { get; set; }
    public Guid? CashBoxId { get; set; }
    public string? CashBoxName { get; set; }
    public Guid? BankAccountId { get; set; }
    public string? BankAccountName { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    public Guid? JournalEntryId { get; set; }
    public string? JournalEntryNumber { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<PaymentVoucherDetailDto> Details { get; set; } = new();
}

public class CreatePaymentVoucherDetailDto
{
    public Guid AccountId { get; set; }
    public Guid? CostCenterId { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public Guid? StakeholderId { get; set; }
    public string? ReferenceInvoiceId { get; set; }
}

public class CreatePaymentVoucherDto
{
    public string? VoucherNumber { get; set; }
    public DateTime VoucherDate { get; set; }
    public Guid? CashBoxId { get; set; }
    public Guid? BankAccountId { get; set; }
    public Guid? FiscalYearId { get; set; }
    public Guid? BranchId { get; set; }
    public string? Notes { get; set; }
    public List<CreatePaymentVoucherDetailDto> Details { get; set; } = new();
}

public class UpdatePaymentVoucherDetailDto
{
    public Guid? Oid { get; set; }
    public Guid AccountId { get; set; }
    public Guid? CostCenterId { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public Guid? StakeholderId { get; set; }
    public string? ReferenceInvoiceId { get; set; }
}

public class UpdatePaymentVoucherDto
{
    public Guid Oid { get; set; }
    public string? VoucherNumber { get; set; }
    public DateTime VoucherDate { get; set; }
    public Guid? CashBoxId { get; set; }
    public Guid? BankAccountId { get; set; }
    public Guid? FiscalYearId { get; set; }
    public Guid? BranchId { get; set; }
    public string? Notes { get; set; }
    public List<UpdatePaymentVoucherDetailDto> Details { get; set; } = new();
}
