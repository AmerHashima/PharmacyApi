using MediatR;
using Pharmacy.Application.DTOs.InvoiceSetup;

namespace Pharmacy.Application.Commands.InvoiceSetup;

public record CreateInvoiceSetupCommand(CreateInvoiceSetupDto Dto) : IRequest<InvoiceSetupDto>;
public record UpdateInvoiceSetupCommand(UpdateInvoiceSetupDto Dto) : IRequest<InvoiceSetupDto>;
public record DeleteInvoiceSetupCommand(Guid Id) : IRequest<bool>;
