using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Application.DTOs.InvoiceShape;

public class UpdateInvoiceShapeDto
{
    [Required]
    public Guid Oid { get; set; }

    [Required]
    [MaxLength(200)]
    public string ShapeName { get; set; } = string.Empty;

    public string HtmlContent { get; set; } = string.Empty;

    public bool DefaultPrint { get; set; } = false;

    public bool IsActive { get; set; } = true;

    [Required]
    public Guid BranchId { get; set; }

    public int? Status { get; set; }
}
