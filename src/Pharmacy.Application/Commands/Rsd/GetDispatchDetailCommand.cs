using MediatR;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Commands.Rsd;

public record GetDispatchDetailCommand(DispatchDetailRequestDto Request) : IRequest<DispatchDetailResponseDto>;