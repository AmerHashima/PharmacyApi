namespace Pharmacy.Application.DTOs.Rsd;

/// <summary>
/// Request DTO for accepting a dispatch notification from RSD/SFDA
/// </summary>
public class AcceptDispatchRequestDto
{
    /// <summary>
    /// The dispatch notification ID from SFDA
    /// </summary>
    public string DispatchNotificationId { get; set; } = string.Empty;

    /// <summary>
    /// Branch ID to determine which integration credentials to use
    /// </summary>
    public Guid BranchId { get; set; }
}