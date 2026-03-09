using MediatR;
using Pharmacy.Application.Commands.Zatca;
using Pharmacy.Application.DTOs.Zatca;
using Pharmacy.Application.Interfaces;

namespace Pharmacy.Application.Handlers.Zatca;

public class ZatcaClearInvoiceHandler : IRequestHandler<ZatcaClearInvoiceCommand, ZatcaSubmitInvoiceResponseDto>
{
    private readonly IZatcaIntegrationService _service;
    public ZatcaClearInvoiceHandler(IZatcaIntegrationService service) => _service = service;

    public Task<ZatcaSubmitInvoiceResponseDto> Handle(ZatcaClearInvoiceCommand request, CancellationToken cancellationToken)
        => _service.ClearInvoiceAsync(request.Request, cancellationToken);
}
