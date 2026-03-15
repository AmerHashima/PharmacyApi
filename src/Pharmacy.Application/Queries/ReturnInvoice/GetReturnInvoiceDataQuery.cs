using Pharmacy.Application.DTOs.ReturnInvoice;
using Pharmacy.Application.DTOs.Common;
using MediatR;

namespace Pharmacy.Application.Queries.ReturnInvoice;

/// <summary>
/// Query to get return invoices with advanced filtering, sorting, and pagination
/// </summary>
public record GetReturnInvoiceDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<ReturnInvoiceDto>>;
