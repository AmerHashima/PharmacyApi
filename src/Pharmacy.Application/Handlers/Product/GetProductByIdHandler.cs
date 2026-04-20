using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        var product = await _repository.GetQueryable()
            .Include(x => x.ProductType)
            .Include(x => x.PackageType)
            .Include(x => x.DosageForm)
            .Include(x => x.VatType)
            .Include(x => x.ProductGroup)
            .Include(x => x.GenericNameRef)
            .Where(x => x.Oid == request.Id && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        return product == null ? null : _mapper.Map<ProductDto>(product);
    }
}
