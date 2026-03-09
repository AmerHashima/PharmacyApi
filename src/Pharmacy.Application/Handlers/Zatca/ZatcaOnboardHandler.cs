using MediatR;
using Pharmacy.Application.Commands.Zatca;
using Pharmacy.Application.DTOs.Zatca;
using Pharmacy.Application.Interfaces;

namespace Pharmacy.Application.Handlers.Zatca;

public class ZatcaOnboardHandler : IRequestHandler<ZatcaOnboardCommand, ZatcaOnboardResponseDto>
{
    private readonly IZatcaIntegrationService _service;
    public ZatcaOnboardHandler(IZatcaIntegrationService service) => _service = service;

    public Task<ZatcaOnboardResponseDto> Handle(ZatcaOnboardCommand request, CancellationToken cancellationToken)
        => _service.OnboardAsync(request.Request, cancellationToken);
}
