using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Application.Queries.StockTransaction;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for getting stock transaction by ID with details
/// </summary>
public class GetStockTransactionByIdHandler : IRequestHandler<GetStockTransactionByIdQuery, StockTransactionWithDetailsDto?>
{
    private readonly IStockTransactionRepository _repository;
    private readonly IMapper _mapper;

    public GetStockTransactionByIdHandler(
        IStockTransactionRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StockTransactionWithDetailsDto?> Handle(GetStockTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return _mapper.Map<StockTransactionWithDetailsDto>(transaction);
    }
}
