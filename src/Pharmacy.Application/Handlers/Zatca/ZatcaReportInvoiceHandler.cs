using MediatR;
using Pharmacy.Application.Commands.Zatca;
using Pharmacy.Application.DTOs.Zatca;
using Pharmacy.Application.Interfaces;

namespace Pharmacy.Application.Handlers.Zatca;

public class ZatcaReportInvoiceHandler : IRequestHandler<ZatcaReportInvoiceCommand, ZatcaSubmitInvoiceResponseDto>
{
    private readonly IZatcaIntegrationService _service;
    public ZatcaReportInvoiceHandler(IZatcaIntegrationService service) => _service = service;

    public Task<ZatcaSubmitInvoiceResponseDto> Handle(ZatcaReportInvoiceCommand request, CancellationToken cancellationToken)
        => _service.ReportInvoiceAsync(request.Request, cancellationToken);
}
