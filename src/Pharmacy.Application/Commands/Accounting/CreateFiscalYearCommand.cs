using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record CreateFiscalYearCommand(CreateFiscalYearDto FiscalYear) : IRequest<FiscalYearDto>;
