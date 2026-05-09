using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Queries.Accounting;

public record GetCostCenterByIdQuery(Guid Id) : IRequest<CostCenterDto?>;
