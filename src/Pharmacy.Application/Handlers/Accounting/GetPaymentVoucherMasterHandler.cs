using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetPaymentVoucherMasterHandler : IRequestHandler<GetPaymentVoucherMasterQuery, PagedResult<PaymentVoucherMasterDto>>
{
    private readonly IPaymentVoucherRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetPaymentVoucherMasterHandler(
        IPaymentVoucherRepository repository,
        IQueryBuilderService queryBuilder,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<PaymentVoucherMasterDto>> Handle(
        GetPaymentVoucherMasterQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(pv => pv.Branch)
            .Include(pv => pv.CashBox)
            .Include(pv => pv.BankAccount)
            .Include(pv => pv.JournalEntry)
            .Where(pv => !pv.IsDeleted);

        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);

        return new PagedResult<PaymentVoucherMasterDto>
        {
            Data            = _mapper.Map<IEnumerable<PaymentVoucherMasterDto>>(paged.Data),
            TotalRecords    = paged.TotalRecords,
            PageNumber      = paged.PageNumber,
            PageSize        = paged.PageSize,
            TotalPages      = paged.TotalPages,
            HasNextPage     = paged.HasNextPage,
            HasPreviousPage = paged.HasPreviousPage
        };
    }
}
