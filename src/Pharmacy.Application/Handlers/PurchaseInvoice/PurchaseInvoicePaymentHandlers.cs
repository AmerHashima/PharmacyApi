using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.PurchaseInvoice;
using Pharmacy.Application.DTOs.PurchaseInvoice;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.PurchaseInvoice;

public class AddPurchaseInvoicePaymentHandler : IRequestHandler<AddPurchaseInvoicePaymentCommand, PurchaseInvoicePaymentDto>
{
    private readonly IPurchaseInvoiceRepository _invoiceRepository;
    private readonly IPurchaseInvoicePaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public AddPurchaseInvoicePaymentHandler(
        IPurchaseInvoiceRepository invoiceRepository,
        IPurchaseInvoicePaymentRepository paymentRepository,
        IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<PurchaseInvoicePaymentDto> Handle(AddPurchaseInvoicePaymentCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.PurchaseInvoiceId, cancellationToken)
            ?? throw new KeyNotFoundException($"Purchase invoice '{request.PurchaseInvoiceId}' not found.");

        var payment = _mapper.Map<PurchaseInvoicePayment>(request.Payment);
        payment.PurchaseInvoiceId = invoice.Oid;
        payment.CreatedAt = DateTime.UtcNow;

        await _paymentRepository.AddAsync(payment, cancellationToken);

        // Update PaidAmount on invoice
        var allPayments = await _paymentRepository.GetByPurchaseInvoiceAsync(invoice.Oid, cancellationToken);
        invoice.PaidAmount = allPayments.Sum(p => p.Amount);
        invoice.UpdatedAt = DateTime.UtcNow;
        await _invoiceRepository.UpdateAsync(invoice, cancellationToken);

        var created = await _paymentRepository.GetByIdAsync(payment.Oid, cancellationToken);
        return _mapper.Map<PurchaseInvoicePaymentDto>(created!);
    }
}

public class DeletePurchaseInvoicePaymentHandler : IRequestHandler<DeletePurchaseInvoicePaymentCommand, bool>
{
    private readonly IPurchaseInvoicePaymentRepository _paymentRepository;
    private readonly IPurchaseInvoiceRepository _invoiceRepository;

    public DeletePurchaseInvoicePaymentHandler(
        IPurchaseInvoicePaymentRepository paymentRepository,
        IPurchaseInvoiceRepository invoiceRepository)
    {
        _paymentRepository = paymentRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<bool> Handle(DeletePurchaseInvoicePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken)
            ?? throw new KeyNotFoundException($"Payment '{request.PaymentId}' not found.");

        await _paymentRepository.DeleteAsync(payment.Oid, cancellationToken);

        // Recalculate PaidAmount
        var invoice = await _invoiceRepository.GetByIdAsync(payment.PurchaseInvoiceId, cancellationToken);
        if (invoice != null)
        {
            var remaining = await _paymentRepository.GetByPurchaseInvoiceAsync(invoice.Oid, cancellationToken);
            invoice.PaidAmount = remaining.Sum(p => p.Amount);
            invoice.UpdatedAt = DateTime.UtcNow;
            await _invoiceRepository.UpdateAsync(invoice, cancellationToken);
        }

        return true;
    }
}
