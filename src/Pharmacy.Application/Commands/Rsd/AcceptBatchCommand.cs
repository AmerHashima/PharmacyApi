using MediatR;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Commands.Rsd;

public record AcceptBatchCommand(AcceptBatchRequestDto Request) : IRequest<AcceptBatchResponseDto>;