using MediatR;
using Pharmacy.Application.DTOs.InvoiceShape;

namespace Pharmacy.Application.Commands.InvoiceShape;

public record CreateInvoiceShapeCommand(CreateInvoiceShapeDto InvoiceShape) : IRequest<InvoiceShapeDto>;
