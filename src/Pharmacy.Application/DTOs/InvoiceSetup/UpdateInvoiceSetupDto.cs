using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.InvoiceSetup;

public class UpdateInvoiceSetupDto
{
    [Required]
    public Guid Oid { get; set; }

    [Required]
    [MaxLength(200)]
    public string NameAr { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string NameEn { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Format { get; set; } = string.Empty;

    public int NumberValue { get; set; }

    public Guid? BranchId { get; set; }

    public Guid? InvoiceTypeId { get; set; }
}
