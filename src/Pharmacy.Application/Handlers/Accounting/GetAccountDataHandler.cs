using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetAccountDataHandler : IRequestHandler<GetAccountDataQuery, PagedResult<AccountDto>>
{
    private readonly IAccountRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetAccountDataHandler(IAccountRepository repository, IQueryBuilderService queryBuilder, IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<AccountDto>> Handle(GetAccountDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(a => a.Parent)
            .Include(a => a.AccountType)
            .Include(a => a.Nature)
            .Where(a => !a.IsDeleted);

        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);
        return new PagedResult<AccountDto>
        {
            Data = _mapper.Map<IEnumerable<AccountDto>>(paged.Data),
            TotalRecords = paged.TotalRecords,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            TotalPages = paged.TotalPages,
            HasNextPage = paged.HasNextPage,
            HasPreviousPage = paged.HasPreviousPage
        };
    }
}
