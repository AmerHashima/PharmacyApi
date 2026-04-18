using MediatR;
using Pharmacy.Application.DTOs.InvoiceShape;

namespace Pharmacy.Application.Commands.InvoiceShape;

public record UpdateInvoiceShapeCommand(UpdateInvoiceShapeDto InvoiceShape) : IRequest<InvoiceShapeDto>;
