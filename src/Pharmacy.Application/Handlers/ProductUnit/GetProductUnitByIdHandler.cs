using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.ProductUnit;
using Pharmacy.Application.Queries.ProductUnit;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.ProductUnit;

public class GetProductUnitByIdHandler : IRequestHandler<GetProductUnitByIdQuery, ProductUnitDto?>
{
    private readonly IProductUnitRepository _repository;
    private readonly IMapper _mapper;

    public GetProductUnitByIdHandler(IProductUnitRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductUnitDto?> Handle(GetProductUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWithDetailsAsync(request.Id, cancellationToken);
        return entity == null ? null : _mapper.Map<ProductUnitDto>(entity);
    }
}
