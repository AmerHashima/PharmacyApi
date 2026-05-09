using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class UpdateCashBoxHandler : IRequestHandler<UpdateCashBoxCommand, CashBoxDto>
{
    private readonly ICashBoxRepository _repository;
    private readonly IMapper _mapper;

    public UpdateCashBoxHandler(ICashBoxRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CashBoxDto> Handle(UpdateCashBoxCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.CashBox.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"CashBox '{request.CashBox.Oid}' not found");

        _mapper.Map(request.CashBox, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(entity, cancellationToken);
        return _mapper.Map<CashBoxDto>(entity);
    }
}
