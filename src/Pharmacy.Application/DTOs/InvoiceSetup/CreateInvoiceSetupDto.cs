using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.InvoiceSetup;

public class CreateInvoiceSetupDto
{
    [Required]
    [MaxLength(200)]
    public string NameAr { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string NameEn { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Format { get; set; } = string.Empty;

    public int NumberValue { get; set; } = 1;

    public Guid? BranchId { get; set; }

    public Guid? InvoiceTypeId { get; set; }
}
