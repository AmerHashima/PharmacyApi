using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record CreatePaymentVoucherCommand(CreatePaymentVoucherDto PaymentVoucher) : IRequest<PaymentVoucherDto>;
