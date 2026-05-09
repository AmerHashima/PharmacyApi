using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record UpdatePaymentVoucherCommand(UpdatePaymentVoucherDto PaymentVoucher) : IRequest<PaymentVoucherDto>;
