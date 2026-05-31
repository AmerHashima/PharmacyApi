using Pharmacy.Domain.Common;
using Pharmacy.Domain.Entities.Accounting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

[Table("PurchaseInvoicePayments")]
public class PurchaseInvoicePayment : BaseEntity
{
    [Required]
    public Guid PurchaseInvoiceId { get; set; }

    [ForeignKey(nameof(PurchaseInvoiceId))]
    public virtual PurchaseInvoice? PurchaseInvoice { get; set; }

    public Guid? PaymentVoucherId { get; set; }

    [ForeignKey(nameof(PaymentVoucherId))]
    public virtual PaymentVoucher? PaymentVoucher { get; set; }

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
    public string? ChequeNumber { get; set; }

    public DateTime PaymentDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
