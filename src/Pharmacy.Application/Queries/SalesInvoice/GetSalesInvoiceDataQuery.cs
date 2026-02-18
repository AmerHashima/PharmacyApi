using Pharmacy.Application.DTOs.SalesInvoice;
using Pharmacy.Application.DTOs.Common;
using MediatR;

namespace Pharmacy.Application.Queries.SalesInvoice;

/// <summary>
/// Query to get sales invoices with advanced filtering, sorting, and pagination
/// </summary>
public record GetSalesInvoiceDataQuery(QueryRequest Request) : IRequest<PagedResult<SalesInvoiceDto>>;
