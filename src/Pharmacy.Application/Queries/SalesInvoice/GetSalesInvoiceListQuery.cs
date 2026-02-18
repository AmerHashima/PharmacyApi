using Pharmacy.Application.DTOs.SalesInvoice;
using MediatR;

namespace Pharmacy.Application.Queries.SalesInvoice;

/// <summary>
/// Query to get sales invoices by branch within a date range
/// </summary>
public record GetSalesInvoiceListQuery(
    Guid? BranchId = null, 
    DateTime? StartDate = null, 
    DateTime? EndDate = null,
    Guid? CashierId = null) : IRequest<IEnumerable<SalesInvoiceDto>>;
