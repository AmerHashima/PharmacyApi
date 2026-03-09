namespace Pharmacy.Application.DTOs.Rsd;

public class StakeholderListRequestDto
{
    public Guid BranchId { get; set; }

    /// <summary>
    /// RSD Stakeholder type: 1=Pharmacy, 2=Supplier, 3=Distributor, 4=Manufacturer, 5=Wholesaler
    /// </summary>
    public int StakeholderType { get; set; } = 1;

    /// <summary>
    /// Whether to get all stakeholders
    /// </summary>
    public bool GetAll { get; set; } = true;

    /// <summary>
    /// Optional city filter
    /// </summary>
    public string? CityId { get; set; }
}
