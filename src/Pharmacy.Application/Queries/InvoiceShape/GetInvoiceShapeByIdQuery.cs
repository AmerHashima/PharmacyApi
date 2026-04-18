using MediatR;
using Pharmacy.Application.DTOs.InvoiceShape;

namespace Pharmacy.Application.Queries.InvoiceShape;

public record GetInvoiceShapeByIdQuery(Guid Id) : IRequest<InvoiceShapeDto?>;
