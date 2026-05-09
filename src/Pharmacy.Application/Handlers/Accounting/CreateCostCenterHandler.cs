using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class CreateCostCenterHandler : IRequestHandler<CreateCostCenterCommand, CostCenterDto>
{
    private readonly ICostCenterRepository _repository;
    private readonly IMapper _mapper;

    public CreateCostCenterHandler(ICostCenterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CostCenterDto> Handle(CreateCostCenterCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Accounting.CostCenter>(request.CostCenter);
        entity.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<CostCenterDto>(entity);
    }
}
