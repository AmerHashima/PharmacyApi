using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.PurchaseInvoice;
using Pharmacy.Application.Queries.PurchaseInvoice;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.PurchaseInvoice;

public class GetPurchaseInvoicesByBranchHandler : IRequestHandler<GetPurchaseInvoicesByBranchQuery, IEnumerable<PurchaseInvoiceDto>>
{
    private readonly IPurchaseInvoiceRepository _repository;
    private readonly IMapper _mapper;

    public GetPurchaseInvoicesByBranchHandler(IPurchaseInvoiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PurchaseInvoiceDto>> Handle(GetPurchaseInvoicesByBranchQuery request, CancellationToken cancellationToken)
    {
        var invoices = await _repository.GetByBranchAsync(request.BranchId, cancellationToken);
        return _mapper.Map<IEnumerable<PurchaseInvoiceDto>>(invoices);
    }
}

public class GetPurchaseInvoicesBySupplierHandler : IRequestHandler<GetPurchaseInvoicesBySupplierQuery, IEnumerable<PurchaseInvoiceDto>>
{
    private readonly IPurchaseInvoiceRepository _repository;
    private readonly IMapper _mapper;

    public GetPurchaseInvoicesBySupplierHandler(IPurchaseInvoiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PurchaseInvoiceDto>> Handle(GetPurchaseInvoicesBySupplierQuery request, CancellationToken cancellationToken)
    {
        var invoices = await _repository.GetBySupplierAsync(request.SupplierId, cancellationToken);
        return _mapper.Map<IEnumerable<PurchaseInvoiceDto>>(invoices);
    }
}

public class GetPurchaseInvoicePaymentsHandler : IRequestHandler<GetPurchaseInvoicePaymentsQuery, IEnumerable<PurchaseInvoicePaymentDto>>
{
    private readonly IPurchaseInvoicePaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public GetPurchaseInvoicePaymentsHandler(IPurchaseInvoicePaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PurchaseInvoicePaymentDto>> Handle(GetPurchaseInvoicePaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetByPurchaseInvoiceAsync(request.PurchaseInvoiceId, cancellationToken);
        return _mapper.Map<IEnumerable<PurchaseInvoicePaymentDto>>(payments);
    }
}
