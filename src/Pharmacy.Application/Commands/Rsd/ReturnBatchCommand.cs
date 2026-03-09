using MediatR;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Commands.Rsd;

public record ReturnBatchCommand(ReturnBatchRequestDto Request) : IRequest<ReturnBatchResponseDto>;
