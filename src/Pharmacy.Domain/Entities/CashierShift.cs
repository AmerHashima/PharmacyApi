using Pharmacy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain.Entities;

[Table("CashierShifts")]
public class CashierShift : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string ShiftNumber { get; set; } = string.Empty;

    [Required]
    public Guid BranchId { get; set; }

    [ForeignKey(nameof(BranchId))]
    public virtual Branch? Branch { get; set; }

    [Required]
    public Guid CashBoxId { get; set; }

    [ForeignKey(nameof(CashBoxId))]
    public virtual Accounting.CashBox? CashBox { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual SystemUser? User { get; set; }

    public DateTime OpenDate { get; set; }

    public DateTime? CloseDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OpeningBalance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ExpectedBalance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ActualBalance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DifferenceAmount { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public virtual ICollection<CashierShiftDetail> Details { get; set; } = new List<CashierShiftDetail>();
    public virtual ICollection<SalesInvoicePayment> SalesInvoicePayments { get; set; } = new List<SalesInvoicePayment>();
}
