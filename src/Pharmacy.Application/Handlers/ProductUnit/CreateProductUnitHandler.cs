using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.ProductUnit;
using Pharmacy.Application.DTOs.ProductUnit;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.ProductUnit;

public class CreateProductUnitHandler : IRequestHandler<CreateProductUnitCommand, ProductUnitDto>
{
    private readonly IProductUnitRepository _repository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateProductUnitHandler(IProductUnitRepository repository, IProductRepository productRepository, IMapper mapper)
    {
        _repository = repository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductUnitDto> Handle(CreateProductUnitCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductUnit.ProductId, cancellationToken)
            ?? throw new KeyNotFoundException($"Product with ID '{request.ProductUnit.ProductId}' not found");

        // Check if this package type already exists for the product
        if (request.ProductUnit.PackageTypeId.HasValue)
        {
            var existing = await _repository.GetByProductAndPackageTypeAsync(
                request.ProductUnit.ProductId, request.ProductUnit.PackageTypeId.Value, cancellationToken);
            if (existing != null)
                throw new InvalidOperationException("This package type already exists for the product");
        }

        var entity = _mapper.Map<Domain.Entities.ProductUnit>(request.ProductUnit);
        var created = await _repository.AddAsync(entity, cancellationToken);

        var result = await _repository.GetWithDetailsAsync(created.Oid, cancellationToken);
        return _mapper.Map<ProductUnitDto>(result);
    }
}
