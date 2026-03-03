using MediatR;
using Pharmacy.Application.Commands.Rsd;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Interfaces;

namespace Pharmacy.Application.Handlers.Rsd;

public class AcceptDispatchHandler : IRequestHandler<AcceptDispatchCommand, AcceptDispatchResponseDto>
{
    private readonly IRsdIntegrationService _rsdService;

    public AcceptDispatchHandler(IRsdIntegrationService rsdService)
    {
        _rsdService = rsdService;
    }

    public async Task<AcceptDispatchResponseDto> Handle(AcceptDispatchCommand request, CancellationToken cancellationToken)
    {
        return await _rsdService.AcceptDispatchAsync(request.Request, cancellationToken);
    }
}