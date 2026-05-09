using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record CreateCostCenterCommand(CreateCostCenterDto CostCenter) : IRequest<CostCenterDto>;
