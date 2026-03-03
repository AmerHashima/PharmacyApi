namespace Pharmacy.Application.DTOs.Rsd;

public class DispatchDetailRequestDto
{
    public string DispatchNotificationId { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
}