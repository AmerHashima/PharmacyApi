using Zatca.Models;

namespace Pharmacy.Application.DTOs.Zatca;

public class ZatcaOnboardRequestDto
{
    public Guid BranchId { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string UnitNameEn { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string OTP { get; set; } = string.Empty;
    /// <summary>1 = Production, 2 = Simulation</summary>
    public string InvoicePortalType { get; set; } = "2";
}
