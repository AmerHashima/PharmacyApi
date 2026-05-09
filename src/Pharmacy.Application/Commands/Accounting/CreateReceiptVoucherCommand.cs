using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record CreateReceiptVoucherCommand(CreateReceiptVoucherDto ReceiptVoucher) : IRequest<ReceiptVoucherDto>;
