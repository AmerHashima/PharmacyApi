using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.AppLookup;

public class CreateAppLookupMasterDto
{
    [Required(ErrorMessage = "Lookup code is required")]
    [StringLength(50, ErrorMessage = "Lookup code cannot exceed 50 characters")]
    public string LookupCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Arabic lookup name is required")]
    [StringLength(100, ErrorMessage = "Arabic lookup name cannot exceed 100 characters")]
    public string LookupNameAr { get; set; } = string.Empty;

    [Required(ErrorMessage = "English lookup name is required")]
    [StringLength(100, ErrorMessage = "English lookup name cannot exceed 100 characters")]
    public string LookupNameEn { get; set; } = string.Empty;

    [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
    public string? Description { get; set; }

    public bool IsSystem { get; set; } = false;
}