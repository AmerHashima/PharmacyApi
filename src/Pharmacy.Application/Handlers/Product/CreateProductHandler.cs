using AutoMapper;
using Pharmacy.Application.Commands.Product;
using Pharmacy.Application.DTOs.Product;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Product;

/// <summary>
/// Handler for creating a new Product
/// </summary>
public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public CreateProductHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Check if GTIN is unique (if provided)
        if (!string.IsNullOrEmpty(request.Product.GTIN))
        {
            if (!await _repository.IsGTINUniqueAsync(request.Product.GTIN, cancellationToken: cancellationToken))
            {
                throw new InvalidOperationException($"GTIN '{request.Product.GTIN}' already exists");
            }
        }

        var product = _mapper.Map<Domain.Entities.Product>(request.Product);
        var createdProduct = await _repository.AddAsync(product, cancellationToken);
        return _mapper.Map<ProductDto>(createdProduct);
    }
}
