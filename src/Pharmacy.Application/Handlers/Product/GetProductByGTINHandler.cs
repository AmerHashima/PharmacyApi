using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Product;
using Pharmacy.Application.Queries.Product;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Product;

/// <summary>
/// Handler for getting product by GTIN
/// </summary>
public class GetProductByGTINHandler : IRequestHandler<GetProductByGTINQuery, ProductDto?>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetProductByGTINHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDto?> Handle(GetProductByGTINQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByGTINAsync(request.GTIN, cancellationToken);
        return _mapper.Map<ProductDto>(product);
    }
}
