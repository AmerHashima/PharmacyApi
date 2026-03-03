using MediatR;
using Pharmacy.Application.Commands.Rsd;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Interfaces;

namespace Pharmacy.Application.Handlers.Rsd;

public class PharmacySaleHandler : IRequestHandler<PharmacySaleCommand, PharmacySaleResponseDto>
{
    private readonly IRsdIntegrationService _rsdService;

    public PharmacySaleHandler(IRsdIntegrationService rsdService)
    {
        _rsdService = rsdService;
    }

    public async Task<PharmacySaleResponseDto> Handle(PharmacySaleCommand request, CancellationToken cancellationToken)
    {
        return await _rsdService.PharmacySaleAsync(request.Request, cancellationToken);
    }
}