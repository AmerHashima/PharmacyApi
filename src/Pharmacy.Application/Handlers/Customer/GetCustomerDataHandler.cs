using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Customer;
using Pharmacy.Application.Queries.Customer;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.Application.Handlers.Customer;

public class GetCustomerDataHandler : IRequestHandler<GetCustomerDataQuery, PagedResult<CustomerDto>>
{
    private readonly ICustomerRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetCustomerDataHandler(ICustomerRepository repository, IQueryBuilderService queryBuilder, IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<CustomerDto>> Handle(GetCustomerDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(c => c.IdentityType)
            .Where(c => !c.IsDeleted);

        var pagedEntities = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);
        var mappedData = _mapper.Map<IEnumerable<CustomerDto>>(pagedEntities.Data);

        return new PagedResult<CustomerDto>
        {
            Data = mappedData,
            TotalRecords = pagedEntities.TotalRecords,
            PageNumber = pagedEntities.PageNumber,
            PageSize = pagedEntities.PageSize,
            TotalPages = pagedEntities.TotalPages,
            HasNextPage = pagedEntities.HasNextPage,
            HasPreviousPage = pagedEntities.HasPreviousPage,
            Metadata = new Dictionary<string, object>
            {
                { "availableFilters",    new List<string> { "NameEN", "NameAR", "Phone", "IdentityNumber", "VatNumber" } },
                { "availableSortFields", new List<string> { "NameEN", "NameAR", "CreatedAt" } }
            }
        };
    }
}
