using Pharmacy.Application.DTOs.Zatca;

namespace Pharmacy.Application.Interfaces;

public interface IZatcaIntegrationService
{
    Task<ZatcaOnboardResponseDto> OnboardAsync(ZatcaOnboardRequestDto request, CancellationToken cancellationToken = default);
    Task<ZatcaSubmitInvoiceResponseDto> ReportInvoiceAsync(ZatcaSubmitInvoiceRequestDto request, CancellationToken cancellationToken = default);
    Task<ZatcaSubmitInvoiceResponseDto> ClearInvoiceAsync(ZatcaSubmitInvoiceRequestDto request, CancellationToken cancellationToken = default);
}
