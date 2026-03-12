using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.ProductUnit;
using Pharmacy.Application.Queries.ProductUnit;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.ProductUnit;

public class GetProductUnitsByProductIdHandler : IRequestHandler<GetProductUnitsByProductIdQuery, IEnumerable<ProductUnitDto>>
{
    private readonly IProductUnitRepository _repository;
    private readonly IMapper _mapper;

    public GetProductUnitsByProductIdHandler(IProductUnitRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductUnitDto>> Handle(GetProductUnitsByProductIdQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByProductIdAsync(request.ProductId, cancellationToken);
        return _mapper.Map<IEnumerable<ProductUnitDto>>(entities);
    }
}
