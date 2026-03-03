using MediatR;
using Pharmacy.Application.Commands.Rsd;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Interfaces;

namespace Pharmacy.Application.Handlers.Rsd;

public class PharmacySaleCancelHandler : IRequestHandler<PharmacySaleCancelCommand, PharmacySaleCancelResponseDto>
{
    private readonly IRsdIntegrationService _rsdService;

    public PharmacySaleCancelHandler(IRsdIntegrationService rsdService)
    {
        _rsdService = rsdService;
    }

    public async Task<PharmacySaleCancelResponseDto> Handle(PharmacySaleCancelCommand request, CancellationToken cancellationToken)
    {
        return await _rsdService.PharmacySaleCancelAsync(request.Request, cancellationToken);
    }
}