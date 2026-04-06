using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Store;

/// <summary>
/// DTO for updating an existing Store
/// </summary>
public class UpdateStoreDto
{
    [Required(ErrorMessage = "Store ID is required")]
    public Guid Oid { get; set; }

    [Required(ErrorMessage = "Store code is required")]
    [MaxLength(50, ErrorMessage = "Store code cannot exceed 50 characters")]
    public string StoreCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Store name is required")]
    [MaxLength(200, ErrorMessage = "Store name cannot exceed 200 characters")]
    public string StoreName { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? Address { get; set; }

    [MaxLength(50, ErrorMessage = "Phone number cannot exceed 50 characters")]
    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Branch ID is required")]
    public Guid BranchId { get; set; }

    public int? Status { get; set; }
}
