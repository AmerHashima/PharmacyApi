using MediatR;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Commands.Rsd;

public record AcceptDispatchCommand(AcceptDispatchRequestDto Request) : IRequest<AcceptDispatchResponseDto>;