using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.InvoiceSetup;

namespace Pharmacy.Application.Queries.InvoiceSetup;

public record GetInvoiceSetupByIdQuery(Guid Id) : IRequest<InvoiceSetupDto?>;
public record GetInvoiceSetupsByBranchQuery(Guid BranchId) : IRequest<IEnumerable<InvoiceSetupDto>>;
public record GetGlobalInvoiceSetupsQuery() : IRequest<IEnumerable<InvoiceSetupDto>>;
public record GetInvoiceSetupDataQuery(QueryRequest Request) : IRequest<PagedResult<InvoiceSetupDto>>;
