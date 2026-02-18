using AutoMapper;
using Pharmacy.Application.DTOs.Stock;
using Pharmacy.Application.Queries.Stock;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.Stock;

/// <summary>
/// Handler for getting stock by branch
/// </summary>
public class GetStockByBranchHandler : IRequestHandler<GetStockByBranchQuery, IEnumerable<StockDto>>
{
    private readonly IStockRepository _repository;
    private readonly IMapper _mapper;

    public GetStockByBranchHandler(IStockRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StockDto>> Handle(GetStockByBranchQuery request, CancellationToken cancellationToken)
    {
        var stocks = await _repository.GetByBranchAsync(request.BranchId, cancellationToken);
        return _mapper.Map<IEnumerable<StockDto>>(stocks);
    }
}
