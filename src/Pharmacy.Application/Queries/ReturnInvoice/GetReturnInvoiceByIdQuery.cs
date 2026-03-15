using Pharmacy.Application.DTOs.ReturnInvoice;
using MediatR;

namespace Pharmacy.Application.Queries.ReturnInvoice;

/// <summary>
/// Query to get a return invoice by ID with items
/// </summary>
public record GetReturnInvoiceByIdQuery(Guid Id) : IRequest<ReturnInvoiceDto?>;
