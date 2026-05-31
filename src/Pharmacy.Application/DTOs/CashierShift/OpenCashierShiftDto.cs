using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.CashierShift;

public class OpenCashierShiftDto
{
    [Required]
    public Guid BranchId { get; set; }

    [Required]
    public Guid CashBoxId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public DateTime OpenDate { get; set; } = DateTime.UtcNow;

    public decimal OpeningBalance { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
