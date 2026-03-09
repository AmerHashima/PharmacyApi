using MediatR;
using Pharmacy.Application.DTOs.Zatca;

namespace Pharmacy.Application.Commands.Zatca;

public record ZatcaOnboardCommand(ZatcaOnboardRequestDto Request) : IRequest<ZatcaOnboardResponseDto>;
