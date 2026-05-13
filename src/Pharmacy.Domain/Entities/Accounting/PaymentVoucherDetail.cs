using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities.Accounting;

[Table("PaymentVoucherDetails", Schema = "Accounting")]
public class PaymentVoucherDetail : BaseEntity
{
    public Guid PaymentVoucherId { get; set; }

    [ForeignKey(nameof(PaymentVoucherId))]
    public virtual PaymentVoucher? PaymentVoucher { get; set; }

    public Guid AccountId { get; set; }

    [ForeignKey(nameof(AccountId))]
    public virtual Account? Account { get; set; }

    public Guid? CostCenterId { get; set; }

    [ForeignKey(nameof(CostCenterId))]
    public virtual CostCenter? CostCenter { get; set; }
    public Guid? StakeholderId { get; set; }

    [ForeignKey(nameof(StakeholderId))]
    public virtual Stakeholder? Stakeholder { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    /// <summary>Optional reference to the source invoice being settled by this line.</summary>
    [MaxLength(100)]
    public string? ReferenceInvoiceId { get; set; }
}
