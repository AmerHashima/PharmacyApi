using MediatR;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Commands.Rsd;

public record GetStakeholderListCommand(StakeholderListRequestDto Request) : IRequest<StakeholderListResponseDto>;
