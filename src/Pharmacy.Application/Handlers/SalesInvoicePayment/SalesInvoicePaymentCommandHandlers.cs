using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.SalesInvoicePayment;
using Pharmacy.Application.DTOs.SalesInvoicePayment;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.SalesInvoicePayment;

public class CreateSalesInvoicePaymentHandler : IRequestHandler<CreateSalesInvoicePaymentCommand, SalesInvoicePaymentDto>
{
    private readonly ISalesInvoicePaymentRepository _paymentRepository;
    private readonly ISalesInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;

    public CreateSalesInvoicePaymentHandler(
        ISalesInvoicePaymentRepository paymentRepository,
        ISalesInvoiceRepository invoiceRepository,
        IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _invoiceRepository = invoiceRepository;
        _mapper = mapper;
    }

    public async Task<SalesInvoicePaymentDto> Handle(CreateSalesInvoicePaymentCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.Payment.SalesInvoiceId, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales invoice '{request.Payment.SalesInvoiceId}' not found.");

        var payment = _mapper.Map<Domain.Entities.SalesInvoicePayment>(request.Payment);
        payment.CreatedAt = DateTime.UtcNow;

        await _paymentRepository.AddAsync(payment, cancellationToken);

        // Update PaidAmount on invoice
        var allPayments = await _paymentRepository.GetBySalesInvoiceAsync(invoice.Oid, cancellationToken);
        invoice.PaidAmount = allPayments.Sum(p => p.Amount);
        invoice.UpdatedAt = DateTime.UtcNow;
        await _invoiceRepository.UpdateAsync(invoice, cancellationToken);

        var created = await _paymentRepository.GetByIdAsync(payment.Oid, cancellationToken);
        return _mapper.Map<SalesInvoicePaymentDto>(created!);
    }
}

public class DeleteSalesInvoicePaymentHandler : IRequestHandler<DeleteSalesInvoicePaymentCommand, bool>
{
    private readonly ISalesInvoicePaymentRepository _paymentRepository;
    private readonly ISalesInvoiceRepository _invoiceRepository;

    public DeleteSalesInvoicePaymentHandler(
        ISalesInvoicePaymentRepository paymentRepository,
        ISalesInvoiceRepository invoiceRepository)
    {
        _paymentRepository = paymentRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<bool> Handle(DeleteSalesInvoicePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken)
            ?? throw new KeyNotFoundException($"Sales invoice payment '{request.PaymentId}' not found.");

        await _paymentRepository.DeleteAsync(payment.Oid, cancellationToken);

        // Recalculate PaidAmount
        var invoice = await _invoiceRepository.GetByIdAsync(payment.SalesInvoiceId, cancellationToken);
        if (invoice != null)
        {
            var remaining = await _paymentRepository.GetBySalesInvoiceAsync(invoice.Oid, cancellationToken);
            invoice.PaidAmount = remaining.Sum(p => p.Amount);
            invoice.UpdatedAt = DateTime.UtcNow;
            await _invoiceRepository.UpdateAsync(invoice, cancellationToken);
        }

        return true;
    }
}
