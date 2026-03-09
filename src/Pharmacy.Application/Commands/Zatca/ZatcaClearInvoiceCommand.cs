using MediatR;
using Pharmacy.Application.DTOs.Zatca;

namespace Pharmacy.Application.Commands.Zatca;

public record ZatcaClearInvoiceCommand(ZatcaSubmitInvoiceRequestDto Request) : IRequest<ZatcaSubmitInvoiceResponseDto>;
