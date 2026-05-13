namespace Pharmacy.Application.DTOs.Accounting;

public class ReceiptVoucherDetailDto
{
    public Guid Oid { get; set; }
    public Guid ReceiptVoucherId { get; set; }
    public Guid AccountId { get; set; }
    public string? AccountCode { get; set; }
    public string? AccountNameAr { get; set; }
    public Guid? CostCenterId { get; set; }
    public string? CostCenterNameAr { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? ReferenceInvoiceId { get; set; }
}

public class ReceiptVoucherDto
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
    public List<ReceiptVoucherDetailDto> Details { get; set; } = new();
}

public class CreateReceiptVoucherDetailDto
{
    public Guid AccountId { get; set; }
    public Guid? CostCenterId { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public Guid? CustomerId { get; set; }
    public string? ReferenceInvoiceId { get; set; }
}

public class CreateReceiptVoucherDto
{
    public string? VoucherNumber { get; set; }
    public DateTime VoucherDate { get; set; }
    public Guid? CashBoxId { get; set; }
    public Guid? BankAccountId { get; set; }
    public Guid? FiscalYearId { get; set; }
    public Guid? BranchId { get; set; }
    public string? Notes { get; set; }
    public List<CreateReceiptVoucherDetailDto> Details { get; set; } = new();
}

public class UpdateReceiptVoucherDetailDto
{
    public Guid? Oid { get; set; }
    public Guid AccountId { get; set; }
    public Guid? CostCenterId { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public Guid? CustomerId { get; set; }
    public string? ReferenceInvoiceId { get; set; }
}

public class UpdateReceiptVoucherDto
{
    public Guid Oid { get; set; }
    public string? VoucherNumber { get; set; }
    public DateTime VoucherDate { get; set; }
    public Guid? CashBoxId { get; set; }
    public Guid? BankAccountId { get; set; }
    public Guid? FiscalYearId { get; set; }
    public Guid? BranchId { get; set; }
    public string? Notes { get; set; }
    public List<UpdateReceiptVoucherDetailDto> Details { get; set; } = new();
}
