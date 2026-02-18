using AutoMapper;
using Pharmacy.Application.DTOs.Product;
using Pharmacy.Application.Queries.Product;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Product;

/// <summary>
/// Handler for getting all products
/// </summary>
public class GetProductListHandler : IRequestHandler<GetProductListQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetProductListHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Product> products;
        
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            products = await _repository.SearchAsync(request.SearchTerm, cancellationToken);
        }
        else if (request.ProductTypeId.HasValue)
        {
            products = await _repository.GetByTypeAsync(request.ProductTypeId.Value, cancellationToken);
        }
        else
        {
            products = await _repository.GetAllAsync(cancellationToken);
        }
        
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}
