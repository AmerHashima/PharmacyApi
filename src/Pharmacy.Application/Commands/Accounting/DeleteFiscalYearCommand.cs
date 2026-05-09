using MediatR;

namespace Pharmacy.Application.Commands.Accounting;

public record DeleteFiscalYearCommand(Guid Id) : IRequest<bool>;
