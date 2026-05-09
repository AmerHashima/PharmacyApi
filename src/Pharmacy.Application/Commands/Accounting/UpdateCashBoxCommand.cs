using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record UpdateCashBoxCommand(UpdateCashBoxDto CashBox) : IRequest<CashBoxDto>;
