using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Queries.Accounting;

public record GetFiscalYearByIdQuery(Guid Id) : IRequest<FiscalYearDto?>;
