using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.PurchaseInvoice;
using Pharmacy.Application.Queries.PurchaseInvoice;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.PurchaseInvoice;

public class GetPurchaseInvoiceDataHandler : IRequestHandler<GetPurchaseInvoiceDataQuery, PagedResult<PurchaseInvoiceDto>>
{
    private readonly IPurchaseInvoiceRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetPurchaseInvoiceDataHandler(
        IPurchaseInvoiceRepository repository,
        IQueryBuilderService queryBuilder,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<PurchaseInvoiceDto>> Handle(GetPurchaseInvoiceDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Where(x => !x.IsDeleted);
        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);
        var dtos = _mapper.Map<List<PurchaseInvoiceDto>>(paged.Data);
        return PagedResult<PurchaseInvoiceDto>.Create(dtos, paged.TotalRecords, paged.PageNumber, paged.PageSize);
    }
}
