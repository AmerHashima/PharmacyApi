using MediatR;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Commands.Rsd;

public record PharmacySaleCommand(PharmacySaleRequestDto Request) : IRequest<PharmacySaleResponseDto>;