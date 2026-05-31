using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

[Table("SalesInvoicePayments")]
public class SalesInvoicePayment : BaseEntity
{
    [Required]
    public Guid SalesInvoiceId { get; set; }

    [ForeignKey(nameof(SalesInvoiceId))]
    public virtual SalesInvoice? SalesInvoice { get; set; }

    public Guid? ShiftId { get; set; }

    [ForeignKey(nameof(ShiftId))]
    public virtual CashierShift? Shift { get; set; }

    [Required]
    public Guid PaymentMethodId { get; set; }

    [ForeignKey(nameof(PaymentMethodId))]
    public virtual AppLookupDetail? PaymentMethod { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    [MaxLength(100)]
    public string? TransactionId { get; set; }

    [MaxLength(100)]
    public string? ApprovalCode { get; set; }

    public DateTime PaymentDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
