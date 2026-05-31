namespace Pharmacy.Application.DTOs.SalesInvoicePayment;

public class SalesInvoicePaymentDto
{
    public Guid Oid { get; set; }
    public Guid SalesInvoiceId { get; set; }
    public string? InvoiceNumber { get; set; }
    public Guid? ShiftId { get; set; }
    public string? ShiftNumber { get; set; }
    public Guid PaymentMethodId { get; set; }
    public string? PaymentMethodName { get; set; }
    public decimal Amount { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? TransactionId { get; set; }
    public string? ApprovalCode { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Notes { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}
