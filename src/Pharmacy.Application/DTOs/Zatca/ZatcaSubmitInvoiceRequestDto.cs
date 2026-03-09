using Zatca.Models;

namespace Pharmacy.Application.DTOs.Zatca;

public class ZatcaSubmitInvoiceRequestDto
{
    public Guid BranchId { get; set; }
    public InvoiceData InvoiceData { get; set; } = null!;
}
