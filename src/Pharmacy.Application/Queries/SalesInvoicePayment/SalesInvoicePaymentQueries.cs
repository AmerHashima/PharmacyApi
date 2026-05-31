using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.SalesInvoicePayment;

namespace Pharmacy.Application.Queries.SalesInvoicePayment;

public record GetSalesInvoicePaymentsByInvoiceQuery(Guid SalesInvoiceId) : IRequest<IEnumerable<SalesInvoicePaymentDto>>;

public record GetSalesInvoicePaymentsByShiftQuery(Guid ShiftId) : IRequest<IEnumerable<SalesInvoicePaymentDto>>;

public record GetSalesInvoicePaymentDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<SalesInvoicePaymentDto>>;
