namespace Pharmacy.Application.DTOs.ProductUnit;

public class ProductUnitDto
{
    public Guid Oid { get; set; }
    public Guid ProductId { get; set; }
    public string? ProductName { get; set; }
    public Guid? PackageTypeId { get; set; }
    public string? PackageTypeName { get; set; }
    public int ConversionFactor { get; set; }
    public decimal? Price { get; set; }
    public string? Barcode { get; set; }
    public DateTime? CreatedAt { get; set; }
}
