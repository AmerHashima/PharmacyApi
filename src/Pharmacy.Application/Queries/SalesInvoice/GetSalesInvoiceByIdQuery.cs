using Pharmacy.Application.DTOs.SalesInvoice;
using MediatR;

namespace Pharmacy.Application.Queries.SalesInvoice;

/// <summary>
/// Query to get a sales invoice by ID with items
/// </summary>
public record GetSalesInvoiceByIdQuery(Guid Id) : IRequest<SalesInvoiceDto?>;
