using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.SalesInvoicePayment;
using Pharmacy.Application.Queries.SalesInvoicePayment;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.SalesInvoicePayment;

public class GetSalesInvoicePaymentsByInvoiceHandler : IRequestHandler<GetSalesInvoicePaymentsByInvoiceQuery, IEnumerable<SalesInvoicePaymentDto>>
{
    private readonly ISalesInvoicePaymentRepository _repository;
    private readonly IMapper _mapper;

    public GetSalesInvoicePaymentsByInvoiceHandler(ISalesInvoicePaymentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SalesInvoicePaymentDto>> Handle(GetSalesInvoicePaymentsByInvoiceQuery request, CancellationToken cancellationToken)
    {
        var payments = await _repository.GetBySalesInvoiceAsync(request.SalesInvoiceId, cancellationToken);
        return _mapper.Map<IEnumerable<SalesInvoicePaymentDto>>(payments);
    }
}

public class GetSalesInvoicePaymentsByShiftHandler : IRequestHandler<GetSalesInvoicePaymentsByShiftQuery, IEnumerable<SalesInvoicePaymentDto>>
{
    private readonly ISalesInvoicePaymentRepository _repository;
    private readonly IMapper _mapper;

    public GetSalesInvoicePaymentsByShiftHandler(ISalesInvoicePaymentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SalesInvoicePaymentDto>> Handle(GetSalesInvoicePaymentsByShiftQuery request, CancellationToken cancellationToken)
    {
        var payments = await _repository.GetByShiftAsync(request.ShiftId, cancellationToken);
        return _mapper.Map<IEnumerable<SalesInvoicePaymentDto>>(payments);
    }
}

public class GetSalesInvoicePaymentDataHandler : IRequestHandler<GetSalesInvoicePaymentDataQuery, PagedResult<SalesInvoicePaymentDto>>
{
    private readonly ISalesInvoicePaymentRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetSalesInvoicePaymentDataHandler(
        ISalesInvoicePaymentRepository repository,
        IQueryBuilderService queryBuilder,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<SalesInvoicePaymentDto>> Handle(GetSalesInvoicePaymentDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Where(x => !x.IsDeleted);
        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);
        var dtos = _mapper.Map<List<SalesInvoicePaymentDto>>(paged.Data);
        return PagedResult<SalesInvoicePaymentDto>.Create(dtos, paged.TotalRecords, paged.PageNumber, paged.PageSize);
    }
}
