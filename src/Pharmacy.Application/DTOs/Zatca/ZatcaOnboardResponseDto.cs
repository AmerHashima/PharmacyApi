namespace Pharmacy.Application.DTOs.Zatca;

public class ZatcaOnboardResponseDto
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? BinarySecurityToken { get; set; }
    public string? Secret { get; set; }
    public string? CertificateContent { get; set; }
    public string? PrivateKeyContent { get; set; }
}
