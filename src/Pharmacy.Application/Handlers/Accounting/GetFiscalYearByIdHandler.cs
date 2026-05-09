using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetFiscalYearByIdHandler : IRequestHandler<GetFiscalYearByIdQuery, FiscalYearDto?>
{
    private readonly IFiscalYearRepository _repository;
    private readonly IMapper _mapper;

    public GetFiscalYearByIdHandler(IFiscalYearRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<FiscalYearDto?> Handle(GetFiscalYearByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return entity is null ? null : _mapper.Map<FiscalYearDto>(entity);
    }
}
