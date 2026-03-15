using Pharmacy.Application.DTOs.ReturnInvoice;
using MediatR;

namespace Pharmacy.Application.Commands.ReturnInvoice;

/// <summary>
/// Command to create a new Return Invoice
/// This will also create stock IN transactions for each returned item
/// </summary>
public record CreateReturnInvoiceCommand(CreateReturnInvoiceDto ReturnInvoice) : IRequest<ReturnInvoiceDto>;
