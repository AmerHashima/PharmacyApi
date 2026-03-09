using MediatR;
using Pharmacy.Application.DTOs.Rsd;

namespace Pharmacy.Application.Queries.Rsd;

public record GetRsdOperationLogByIdQuery(Guid Id) : IRequest<RsdOperationLogWithDetailsDto?>;
