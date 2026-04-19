namespace Pharmacy.Application.DTOs.Rsd;

/// <summary>
/// Request DTO for SFDA RSD DrugList sync operation
/// </summary>
public class DrugListRequestDto
{
    /// <summary>
    /// Branch ID used to resolve RSD credentials
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Drug status filter: 1=Registered, 2=Cancelled, 3=Suspended
    /// </summary>
    public int DrugStatus { get; set; } = 1;
}
