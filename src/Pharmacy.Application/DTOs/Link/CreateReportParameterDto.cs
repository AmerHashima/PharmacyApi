using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.Link;

public class CreateReportParameterDto
{
    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }

    public bool? Active { get; set; }

    public int? Type { get; set; }

    [MaxLength(500)]
    public string? DefaultValue { get; set; }
}
