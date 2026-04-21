using MediatR;
using Pharmacy.Application.DTOs.Zatca;

namespace Pharmacy.Application.Commands.Zatca;

public record ZatcaReportSalesInvoiceCommand(Guid InvoiceId) : IRequest<ZatcaSubmitInvoiceResponseDto>;
