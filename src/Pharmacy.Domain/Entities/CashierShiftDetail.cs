using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

[Table("CashierShiftDetails")]
public class CashierShiftDetail : BaseEntity
{
    [Required]
    public Guid ShiftId { get; set; }

    [ForeignKey(nameof(ShiftId))]
    public virtual CashierShift? Shift { get; set; }

    public DateTime TransactionDate { get; set; }

    public Guid? TransactionTypeId { get; set; }

    [ForeignKey(nameof(TransactionTypeId))]
    public virtual AppLookupDetail? TransactionType { get; set; }

    public Guid? ReferenceId { get; set; }

    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    public Guid? PaymentMethodId { get; set; }

    [ForeignKey(nameof(PaymentMethodId))]
    public virtual AppLookupDetail? PaymentMethod { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
