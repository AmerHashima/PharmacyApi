using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetCostCenterByIdHandler : IRequestHandler<GetCostCenterByIdQuery, CostCenterDto?>
{
    private readonly ICostCenterRepository _repository;
    private readonly IMapper _mapper;

    public GetCostCenterByIdHandler(ICostCenterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CostCenterDto?> Handle(GetCostCenterByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetQueryable()
            .Include(c => c.Parent)
            .Where(c => c.Oid == request.Id && !c.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        return entity is null ? null : _mapper.Map<CostCenterDto>(entity);
    }
}
