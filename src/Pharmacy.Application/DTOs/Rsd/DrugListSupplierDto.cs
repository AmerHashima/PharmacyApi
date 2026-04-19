namespace Pharmacy.Application.DTOs.Rsd;

/// <summary>
/// A supplier entry inside a drug record from SFDA RSD DrugListService
/// </summary>
public class DrugListSupplierDto
{
    public string? GLN { get; set; }
    public string? SupplierName { get; set; }
}
