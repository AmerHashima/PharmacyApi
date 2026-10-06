using Pharmacy.Application.DTOs.ReturnInvoice;
using MediatR;
using Pharmacy.Application.Common.Interfaces;

namespace Pharmacy.Application.Commands.ReturnInvoice;

/// <summary>
/// Command to create a new Return Invoice
/// This will also create stock IN transactions for each returned item
/// </summary>
public record CreateReturnInvoiceCommand(CreateReturnInvoiceDto ReturnInvoice) : IRequest<ReturnInvoiceDto>, ITransactionalRequest;
