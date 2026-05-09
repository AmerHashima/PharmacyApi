using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class CreateCashBoxHandler : IRequestHandler<CreateCashBoxCommand, CashBoxDto>
{
    private readonly ICashBoxRepository _repository;
    private readonly IMapper _mapper;

    public CreateCashBoxHandler(ICashBoxRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CashBoxDto> Handle(CreateCashBoxCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Accounting.CashBox>(request.CashBox);
        entity.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<CashBoxDto>(entity);
    }
}
