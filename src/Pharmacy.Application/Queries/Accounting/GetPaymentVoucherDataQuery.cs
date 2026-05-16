using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;

namespace Pharmacy.Application.Queries.Accounting;

public record GetPaymentVoucherDataQuery(QueryRequest QueryRequest) : IRequest<PagedResult<PaymentVoucherDto>>;

public record GetPaymentVoucherMasterQuery(QueryRequest QueryRequest) : IRequest<PagedResult<PaymentVoucherMasterDto>>;
