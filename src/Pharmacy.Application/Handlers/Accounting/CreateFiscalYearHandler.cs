using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class CreateFiscalYearHandler : IRequestHandler<CreateFiscalYearCommand, FiscalYearDto>
{
    private readonly IFiscalYearRepository _repository;
    private readonly IMapper _mapper;

    public CreateFiscalYearHandler(IFiscalYearRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<FiscalYearDto> Handle(CreateFiscalYearCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.Accounting.FiscalYear>(request.FiscalYear);
        entity.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<FiscalYearDto>(entity);
    }
}
