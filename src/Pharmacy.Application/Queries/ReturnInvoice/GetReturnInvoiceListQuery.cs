using Pharmacy.Application.DTOs.ReturnInvoice;
using MediatR;

namespace Pharmacy.Application.Queries.ReturnInvoice;

/// <summary>
/// Query to get return invoices by branch within a date range
/// </summary>
public record GetReturnInvoiceListQuery(
    Guid? BranchId = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    Guid? CashierId = null) : IRequest<IEnumerable<ReturnInvoiceDto>>;
