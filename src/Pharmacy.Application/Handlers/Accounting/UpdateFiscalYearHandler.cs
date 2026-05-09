using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class UpdateFiscalYearHandler : IRequestHandler<UpdateFiscalYearCommand, FiscalYearDto>
{
    private readonly IFiscalYearRepository _repository;
    private readonly IMapper _mapper;

    public UpdateFiscalYearHandler(IFiscalYearRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<FiscalYearDto> Handle(UpdateFiscalYearCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.FiscalYear.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"FiscalYear '{request.FiscalYear.Oid}' not found");

        _mapper.Map(request.FiscalYear, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(entity, cancellationToken);
        return _mapper.Map<FiscalYearDto>(entity);
    }
}
