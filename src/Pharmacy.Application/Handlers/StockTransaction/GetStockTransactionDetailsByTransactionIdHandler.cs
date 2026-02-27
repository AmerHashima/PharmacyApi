using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Application.Queries.StockTransaction;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for getting all details for a specific stock transaction
/// </summary>
public class GetStockTransactionDetailsByTransactionIdHandler : IRequestHandler<GetStockTransactionDetailsByTransactionIdQuery, IEnumerable<StockTransactionDetailDto>>
{
    private readonly IStockTransactionDetailRepository _repository;
    private readonly IMapper _mapper;

    public GetStockTransactionDetailsByTransactionIdHandler(
        IStockTransactionDetailRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StockTransactionDetailDto>> Handle(GetStockTransactionDetailsByTransactionIdQuery request, CancellationToken cancellationToken)
    {
        var details = await _repository.GetByTransactionIdAsync(request.TransactionId, cancellationToken);
        return _mapper.Map<IEnumerable<StockTransactionDetailDto>>(details);
    }
}
