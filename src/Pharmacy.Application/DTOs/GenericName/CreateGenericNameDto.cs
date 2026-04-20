using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.GenericName;

public class CreateGenericNameDto
{
    [Required]
    [MaxLength(200)]
    public string NameEN { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string NameAR { get; set; } = string.Empty;
}
