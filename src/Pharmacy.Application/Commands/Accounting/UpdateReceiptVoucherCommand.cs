using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Commands.Accounting;

public record UpdateReceiptVoucherCommand(UpdateReceiptVoucherDto ReceiptVoucher) : IRequest<ReceiptVoucherDto>;
