using MediatR;

namespace Pharmacy.Application.Commands.Accounting;

public record DeleteReceiptVoucherCommand(Guid Id) : IRequest<bool>;
