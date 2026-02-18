using AutoMapper;
using Pharmacy.Application.Commands.Product;
using Pharmacy.Application.DTOs.Product;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Product;

/// <summary>
/// Handler for updating an existing Product
/// </summary>
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public UpdateProductHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await _repository.GetByIdAsync(request.Product.Oid, cancellationToken);
        if (existingProduct == null)
        {
            throw new KeyNotFoundException($"Product with ID '{request.Product.Oid}' not found");
        }

        // Check if GTIN is unique (if provided, excluding current product)
        if (!string.IsNullOrEmpty(request.Product.GTIN))
        {
            if (!await _repository.IsGTINUniqueAsync(request.Product.GTIN, request.Product.Oid, cancellationToken))
            {
                throw new InvalidOperationException($"GTIN '{request.Product.GTIN}' already exists");
            }
        }

        _mapper.Map(request.Product, existingProduct);
        await _repository.UpdateAsync(existingProduct, cancellationToken);
        return _mapper.Map<ProductDto>(existingProduct);
    }
}
