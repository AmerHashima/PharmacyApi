namespace Pharmacy.Application.DTOs.InvoiceSetup;

public class InvoiceSetupDto
{
    public Guid Oid { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public int NumberValue { get; set; }
    public Guid? BranchId { get; set; }
    public string? BranchName { get; set; }
    public Guid? InvoiceTypeId { get; set; }
    public string? InvoiceTypeName { get; set; }
    public DateTime? CreatedAt { get; set; }
}
