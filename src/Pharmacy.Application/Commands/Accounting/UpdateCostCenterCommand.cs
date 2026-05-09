using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record UpdateCostCenterCommand(UpdateCostCenterDto CostCenter) : IRequest<CostCenterDto>;
