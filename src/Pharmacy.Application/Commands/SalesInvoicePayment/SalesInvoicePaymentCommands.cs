using MediatR;
using Pharmacy.Application.DTOs.SalesInvoicePayment;

namespace Pharmacy.Application.Commands.SalesInvoicePayment;

public record CreateSalesInvoicePaymentCommand(CreateSalesInvoicePaymentDto Payment) : IRequest<SalesInvoicePaymentDto>;

public record DeleteSalesInvoicePaymentCommand(Guid PaymentId) : IRequest<bool>;
