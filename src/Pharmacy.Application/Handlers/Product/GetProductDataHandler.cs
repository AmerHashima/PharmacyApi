using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.DTOs.Product;
using Pharmacy.Application.Queries.Product;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Product;

/// <summary>
/// Handler for getting products with advanced filtering, sorting, and pagination
/// </summary>
public class GetProductDataHandler : IRequestHandler<GetProductDataQuery, PagedResult<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly IQueryBuilderService _queryBuilderService;
    private readonly IMapper _mapper;

    public GetProductDataHandler(
        IProductRepository repository,
        IQueryBuilderService queryBuilderService,
        IMapper mapper)
    {
        _repository = repository;
        _queryBuilderService = queryBuilderService;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProductDto>> Handle(GetProductDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
                        .Include(x => x.ProductType)
                        .Include(x => x.VatType)
                        .Include(x => x.ProductGroup)
.Where(x => !x.IsDeleted);

        // Apply filters
        query = _queryBuilderService.ApplyFilters(query, request.Request.Request.Filters);

        // Apply sorting
        query = _queryBuilderService.ApplySorting(query, request.Request.Request.Sort);

        // Apply pagination and get result
        var pagedResult = await _queryBuilderService.ApplyPaginationAsync(query, request.Request.Request.Pagination);

        var productDtos = _mapper.Map<List<ProductDto>>(pagedResult.Data);

        return PagedResult<ProductDto>.Create(
            productDtos,
            pagedResult.TotalRecords,
            pagedResult.PageNumber,
            pagedResult.PageSize);
    }
}
