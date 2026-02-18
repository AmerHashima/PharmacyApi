using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.AppLookup;

/// <summary>
/// DTO for updating an existing AppLookupDetail
/// </summary>
public class UpdateAppLookupDetailDto
{
    [Required(ErrorMessage = "ID is required")]
    public Guid Oid { get; set; }

    [Required(ErrorMessage = "Lookup master ID is required")]
    public Guid LookupMasterID { get; set; }

    [Required(ErrorMessage = "Value code is required")]
    [StringLength(50, ErrorMessage = "Value code cannot exceed 50 characters")]
    public string ValueCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Arabic value name is required")]
    [StringLength(100, ErrorMessage = "Arabic value name cannot exceed 100 characters")]
    public string ValueNameAr { get; set; } = string.Empty;

    [Required(ErrorMessage = "English value name is required")]
    [StringLength(100, ErrorMessage = "English value name cannot exceed 100 characters")]
    public string ValueNameEn { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Sort order must be greater than 0")]
    public int SortOrder { get; set; } = 1;

    public bool IsDefault { get; set; } = false;

    public bool IsActive { get; set; } = true;
}
