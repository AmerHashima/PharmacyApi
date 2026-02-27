using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Application.Queries.StockTransaction;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for getting stock transaction detail by ID
/// </summary>
public class GetStockTransactionDetailByIdHandler : IRequestHandler<GetStockTransactionDetailByIdQuery, StockTransactionDetailDto?>
{
    private readonly IStockTransactionDetailRepository _repository;
    private readonly IMapper _mapper;

    public GetStockTransactionDetailByIdHandler(
        IStockTransactionDetailRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StockTransactionDetailDto?> Handle(GetStockTransactionDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var detail = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return _mapper.Map<StockTransactionDetailDto>(detail);
    }
}
