using MediatR;
using Pharmacy.Application.DTOs.Accounting;

namespace Pharmacy.Application.Queries.Accounting;

public record GetReceiptVoucherByIdQuery(Guid Id) : IRequest<ReceiptVoucherDto?>;
