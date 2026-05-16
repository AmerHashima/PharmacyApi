using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetReceiptVoucherMasterHandler : IRequestHandler<GetReceiptVoucherMasterQuery, PagedResult<ReceiptVoucherMasterDto>>
{
    private readonly IReceiptVoucherRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetReceiptVoucherMasterHandler(
        IReceiptVoucherRepository repository,
        IQueryBuilderService queryBuilder,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<ReceiptVoucherMasterDto>> Handle(
        GetReceiptVoucherMasterQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(rv => rv.Branch)
            .Include(rv => rv.CashBox)
            .Include(rv => rv.BankAccount)
            .Include(rv => rv.JournalEntry)
            .Where(rv => !rv.IsDeleted);

        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);

        return new PagedResult<ReceiptVoucherMasterDto>
        {
            Data            = _mapper.Map<IEnumerable<ReceiptVoucherMasterDto>>(paged.Data),
            TotalRecords    = paged.TotalRecords,
            PageNumber      = paged.PageNumber,
            PageSize        = paged.PageSize,
            TotalPages      = paged.TotalPages,
            HasNextPage     = paged.HasNextPage,
            HasPreviousPage = paged.HasPreviousPage
        };
    }
}
