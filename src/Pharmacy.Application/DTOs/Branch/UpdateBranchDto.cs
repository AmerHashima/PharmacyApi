using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Branch;

/// <summary>
/// DTO for updating an existing Branch
/// </summary>
public class UpdateBranchDto
{
    [Required]
    public Guid Oid { get; set; }

    [Required(ErrorMessage = "Branch code is required")]
    [MaxLength(50, ErrorMessage = "Branch code cannot exceed 50 characters")]
    public string BranchCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Branch name is required")]
    [MaxLength(200, ErrorMessage = "Branch name cannot exceed 200 characters")]
    public string BranchName { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "GLN cannot exceed 20 characters")]
    public string? GLN { get; set; }

    [MaxLength(20, ErrorMessage = "CRN cannot exceed 20 characters")]
    public string? CRN { get; set; }

    [MaxLength(20, ErrorMessage = "Vat number cannot exceed 20 characters")]
    public string? VatNumber { get; set; }

    public Guid? IdentifyLookupId { get; set; }
    public string? IdentifyValue { get; set; }

    [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    public string? City { get; set; }

    [MaxLength(100, ErrorMessage = "District cannot exceed 100 characters")]
    public string? District { get; set; }

    [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? Address { get; set; }

    [MaxLength(100, ErrorMessage = "Street name cannot exceed 100 characters")]
    public string? StreetName { get; set; }

    [MaxLength(10, ErrorMessage = "Building number cannot exceed 10 characters")]
    public string? BuildingNumber { get; set; }

    [MaxLength(100, ErrorMessage = "City subdivision name cannot exceed 100 characters")]
    public string? CitySubdivisionName { get; set; }

    [MaxLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
    public string? CityName { get; set; }

    [MaxLength(10, ErrorMessage = "Postal zone cannot exceed 10 characters")]
    public string? PostalZone { get; set; }

    [MaxLength(200, ErrorMessage = "Registration name cannot exceed 200 characters")]
    public string? RegistrationName { get; set; }

    public int? Status { get; set; }
}

