using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.CashierShift;

public class CloseCashierShiftDto
{
    public DateTime CloseDate { get; set; } = DateTime.UtcNow;

    public decimal? ActualBalance { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}

public class AddCashierShiftDetailDto
{
    [Required]
    public Guid ShiftId { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    public Guid? TransactionTypeId { get; set; }
    public Guid? ReferenceId { get; set; }

    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    public Guid? PaymentMethodId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
