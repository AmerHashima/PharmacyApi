using Pharmacy.Application.DTOs.SalesInvoice;
using MediatR;

namespace Pharmacy.Application.Commands.SalesInvoice;

/// <summary>
/// Command to create a new Sales Invoice (POS transaction)
/// This will also create stock OUT transactions for each item
/// </summary>
public record CreateSalesInvoiceCommand(CreateSalesInvoiceDto Invoice) : IRequest<SalesInvoiceDto>;
