using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.PurchaseInvoice;

namespace Pharmacy.Application.Queries.PurchaseInvoice;

public record GetPurchaseInvoiceByIdQuery(Guid Id) : IRequest<PurchaseInvoiceDto?>;

public record GetPurchaseInvoiceDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<PurchaseInvoiceDto>>;

public record GetPurchaseInvoicesByBranchQuery(Guid BranchId) : IRequest<IEnumerable<PurchaseInvoiceDto>>;

public record GetPurchaseInvoicesBySupplierQuery(Guid SupplierId) : IRequest<IEnumerable<PurchaseInvoiceDto>>;

public record GetPurchaseInvoicePaymentsQuery(Guid PurchaseInvoiceId) : IRequest<IEnumerable<PurchaseInvoicePaymentDto>>;
