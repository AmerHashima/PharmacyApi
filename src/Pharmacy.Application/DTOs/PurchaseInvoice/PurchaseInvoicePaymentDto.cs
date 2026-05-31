namespace Pharmacy.Application.DTOs.PurchaseInvoice;

public class PurchaseInvoicePaymentDto
{
    public Guid Oid { get; set; }
    public Guid PurchaseInvoiceId { get; set; }
    public Guid? PaymentVoucherId { get; set; }
    public Guid PaymentMethodId { get; set; }
    public string? PaymentMethodName { get; set; }
    public decimal Amount { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? TransactionId { get; set; }
    public string? ChequeNumber { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Notes { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}
