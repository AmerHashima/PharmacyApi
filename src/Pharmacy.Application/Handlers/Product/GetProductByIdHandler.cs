using AutoMapper;
using Pharmacy.Application.DTOs.Product;
using Pharmacy.Application.Queries.Product;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Product;

/// <summary>
/// Handler for getting a product by ID
/// </summary>
public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetProductByIdHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }
}
