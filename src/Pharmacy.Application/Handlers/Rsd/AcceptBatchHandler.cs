using MediatR;
using Pharmacy.Application.Commands.Rsd;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Interfaces;

namespace Pharmacy.Application.Handlers.Rsd;

public class AcceptBatchHandler : IRequestHandler<AcceptBatchCommand, AcceptBatchResponseDto>
{
    private readonly IRsdIntegrationService _rsdService;

    public AcceptBatchHandler(IRsdIntegrationService rsdService)
    {
        _rsdService = rsdService;
    }

    public async Task<AcceptBatchResponseDto> Handle(AcceptBatchCommand request, CancellationToken cancellationToken)
    {
        return await _rsdService.AcceptBatchAsync(request.Request, cancellationToken);
    }
}