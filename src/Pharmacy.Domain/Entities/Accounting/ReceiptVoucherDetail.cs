using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities.Accounting;

[Table("ReceiptVoucherDetails", Schema = "Accounting")]
public class ReceiptVoucherDetail : BaseEntity
{
    public Guid ReceiptVoucherId { get; set; }

    [ForeignKey(nameof(ReceiptVoucherId))]
    public virtual ReceiptVoucher? ReceiptVoucher { get; set; }

    public Guid AccountId { get; set; }

    [ForeignKey(nameof(AccountId))]
    public virtual Account? Account { get; set; }

    public Guid? CostCenterId { get; set; }

    [ForeignKey(nameof(CostCenterId))]
    public virtual CostCenter? CostCenter { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public Guid? CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public virtual Customer? Customer { get; set; }

    /// <summary>Optional reference to the source invoice being settled by this line.</summary>
    [MaxLength(100)]
    public string? ReferenceInvoiceId { get; set; }
}
