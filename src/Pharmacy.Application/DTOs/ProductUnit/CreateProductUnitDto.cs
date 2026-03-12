using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.ProductUnit;

public class CreateProductUnitDto
{
    [Required(ErrorMessage = "Product ID is required")]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "Package type is required")]
    public Guid? PackageTypeId { get; set; }

    [Required(ErrorMessage = "Conversion factor is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Conversion factor must be at least 1")]
    public int ConversionFactor { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
    public decimal? Price { get; set; }

    [MaxLength(100)]
    public string? Barcode { get; set; }
}
