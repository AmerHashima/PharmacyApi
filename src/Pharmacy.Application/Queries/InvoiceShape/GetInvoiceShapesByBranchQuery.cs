using MediatR;
using Pharmacy.Application.DTOs.InvoiceShape;

namespace Pharmacy.Application.Queries.InvoiceShape;

public record GetInvoiceShapesByBranchQuery(Guid BranchId) : IRequest<IEnumerable<InvoiceShapeDto>>;
