using MediatR;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Commands.Rsd;

public record SyncDrugListCommand(DrugListRequestDto Request) : IRequest<DrugListSyncResponseDto>;
