using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Queries.Accounting;

public record GetPaymentVoucherByIdQuery(Guid Id) : IRequest<PaymentVoucherDto?>;
