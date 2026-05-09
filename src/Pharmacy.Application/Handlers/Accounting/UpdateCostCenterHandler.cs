using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class UpdateCostCenterHandler : IRequestHandler<UpdateCostCenterCommand, CostCenterDto>
{
    private readonly ICostCenterRepository _repository;
    private readonly IMapper _mapper;

    public UpdateCostCenterHandler(ICostCenterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CostCenterDto> Handle(UpdateCostCenterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.CostCenter.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"CostCenter '{request.CostCenter.Oid}' not found");

        _mapper.Map(request.CostCenter, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(entity, cancellationToken);
        return _mapper.Map<CostCenterDto>(entity);
    }
}
