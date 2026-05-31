namespace Pharmacy.Application.DTOs.CashierShift;

public class CashierShiftDetailDto
{
    public Guid Oid { get; set; }
    public Guid ShiftId { get; set; }
    public DateTime TransactionDate { get; set; }
    public Guid? TransactionTypeId { get; set; }
    public string? TransactionTypeName { get; set; }
    public Guid? ReferenceId { get; set; }
    public string? ReferenceNumber { get; set; }
    public Guid? PaymentMethodId { get; set; }
    public string? PaymentMethodName { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
    public DateTime? CreatedAt { get; set; }
}
