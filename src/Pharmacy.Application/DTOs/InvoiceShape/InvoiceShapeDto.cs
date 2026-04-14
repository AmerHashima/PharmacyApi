namespace Pharmacy.Application.DTOs.InvoiceShape;

public class InvoiceShapeDto
{
    public Guid Oid { get; set; }
    public string ShapeName { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public bool DefaultPrint { get; set; }
    public bool IsActive { get; set; }
    public Guid BranchId { get; set; }
    public string? BranchName { get; set; }
    public DateTime? CreatedAt { get; set; }
}
