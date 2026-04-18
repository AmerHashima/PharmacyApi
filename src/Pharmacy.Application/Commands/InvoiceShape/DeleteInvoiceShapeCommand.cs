using MediatR;

namespace Pharmacy.Application.Commands.InvoiceShape;

public record DeleteInvoiceShapeCommand(Guid Id) : IRequest<bool>;
