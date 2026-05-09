using MediatR;

namespace Pharmacy.Application.Commands.Accounting;

public record DeletePaymentVoucherCommand(Guid Id) : IRequest<bool>;
