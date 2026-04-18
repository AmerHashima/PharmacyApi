using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.InvoiceShape;

namespace Pharmacy.Application.Queries.InvoiceShape;

public record GetInvoiceShapeDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<InvoiceShapeDto>>;
