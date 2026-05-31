using MediatR;
using Pharmacy.Application.DTOs.PurchaseInvoice;

namespace Pharmacy.Application.Commands.PurchaseInvoice;

public record CreatePurchaseInvoiceCommand(CreatePurchaseInvoiceDto Invoice) : IRequest<PurchaseInvoiceDto>;

public record UpdatePurchaseInvoiceCommand(Guid Id, UpdatePurchaseInvoiceDto Invoice) : IRequest<PurchaseInvoiceDto>;

public record DeletePurchaseInvoiceCommand(Guid Id) : IRequest<bool>;

public record AddPurchaseInvoicePaymentCommand(Guid PurchaseInvoiceId, CreatePurchaseInvoicePaymentDto Payment) : IRequest<PurchaseInvoicePaymentDto>;

public record DeletePurchaseInvoicePaymentCommand(Guid PaymentId) : IRequest<bool>;
