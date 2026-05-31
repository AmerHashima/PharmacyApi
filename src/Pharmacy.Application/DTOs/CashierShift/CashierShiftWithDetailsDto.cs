namespace Pharmacy.Application.DTOs.CashierShift;

public class CashierShiftWithDetailsDto
{
    public Guid Oid { get; set; }
    public string ShiftNumber { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
    public string? BranchName { get; set; }
    public Guid CashBoxId { get; set; }
    public string? CashBoxName { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal? ExpectedBalance { get; set; }
    public decimal? ActualBalance { get; set; }
    public decimal? DifferenceAmount { get; set; }
    public string? Notes { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<CashierShiftDetailDto> Details { get; set; } = new();
}
