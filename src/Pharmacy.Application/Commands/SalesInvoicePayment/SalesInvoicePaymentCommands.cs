using MediatR;
using Pharmacy.Application.DTOs.SalesInvoicePayment;
using Pharmacy.Application.Common.Interfaces;

namespace Pharmacy.Application.Commands.SalesInvoicePayment;

public record CreateSalesInvoicePaymentCommand(CreateSalesInvoicePaymentDto Payment) : IRequest<SalesInvoicePaymentDto>, ITransactionalRequest;

public record DeleteSalesInvoicePaymentCommand(Guid PaymentId) : IRequest<bool>, ITransactionalRequest;
