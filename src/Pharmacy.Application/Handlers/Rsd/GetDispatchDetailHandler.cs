using MediatR;
using Pharmacy.Application.Commands.Rsd;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Interfaces;

namespace Pharmacy.Application.Handlers.Rsd;

public class GetDispatchDetailHandler : IRequestHandler<GetDispatchDetailCommand, DispatchDetailResponseDto>
{
    private readonly IRsdIntegrationService _rsdService;

    public GetDispatchDetailHandler(IRsdIntegrationService rsdService)
    {
        _rsdService = rsdService;
    }

    public async Task<DispatchDetailResponseDto> Handle(GetDispatchDetailCommand request, CancellationToken cancellationToken)
    {
        return await _rsdService.GetDispatchDetailAsync(request.Request, cancellationToken);
    }
}