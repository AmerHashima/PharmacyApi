using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetCashBoxByIdHandler : IRequestHandler<GetCashBoxByIdQuery, CashBoxDto?>
{
    private readonly ICashBoxRepository _repository;
    private readonly IMapper _mapper;

    public GetCashBoxByIdHandler(ICashBoxRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CashBoxDto?> Handle(GetCashBoxByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetQueryable()
            .Include(cb => cb.Branch)
            .Include(cb => cb.Account)
            .Where(cb => cb.Oid == request.Id && !cb.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        return entity is null ? null : _mapper.Map<CashBoxDto>(entity);
    }
}
