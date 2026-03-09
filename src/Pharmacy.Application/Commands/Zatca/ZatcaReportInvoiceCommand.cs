using MediatR;
using Pharmacy.Application.DTOs.Zatca;

namespace Pharmacy.Application.Commands.Zatca;

public record ZatcaReportInvoiceCommand(ZatcaSubmitInvoiceRequestDto Request) : IRequest<ZatcaSubmitInvoiceResponseDto>;
