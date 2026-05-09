using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record UpdateFiscalYearCommand(UpdateFiscalYearDto FiscalYear) : IRequest<FiscalYearDto>;
