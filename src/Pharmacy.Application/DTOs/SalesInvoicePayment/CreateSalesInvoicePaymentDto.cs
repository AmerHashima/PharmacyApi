using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.SalesInvoicePayment;

public class CreateSalesInvoicePaymentDto
{
    [Required]
    public Guid SalesInvoiceId { get; set; }

    public Guid? ShiftId { get; set; }

    [Required]
    public Guid PaymentMethodId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    [MaxLength(100)]
    public string? TransactionId { get; set; }

    [MaxLength(100)]
    public string? ApprovalCode { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? Notes { get; set; }
}
