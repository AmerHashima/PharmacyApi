using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;

namespace Pharmacy.Application.Queries.Accounting;

public record GetReceiptVoucherDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<ReceiptVoucherDto>>;
