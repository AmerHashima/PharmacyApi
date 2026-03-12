using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.ProductUnit;
using Pharmacy.Application.DTOs.ProductUnit;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.ProductUnit;

public class UpdateProductUnitHandler : IRequestHandler<UpdateProductUnitCommand, ProductUnitDto>
{
    private readonly IProductUnitRepository _repository;
    private readonly IMapper _mapper;

    public UpdateProductUnitHandler(IProductUnitRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductUnitDto> Handle(UpdateProductUnitCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.ProductUnit.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"ProductUnit with ID '{request.ProductUnit.Oid}' not found");

        _mapper.Map(request.ProductUnit, existing);
        await _repository.UpdateAsync(existing, cancellationToken);

        var result = await _repository.GetWithDetailsAsync(existing.Oid, cancellationToken);
        return _mapper.Map<ProductUnitDto>(result);
    }
}
