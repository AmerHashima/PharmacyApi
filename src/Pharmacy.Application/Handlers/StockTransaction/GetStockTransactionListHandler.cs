using AutoMapper;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Application.Queries.StockTransaction;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for getting stock transactions list
/// </summary>
public class GetStockTransactionListHandler : IRequestHandler<GetStockTransactionListQuery, IEnumerable<StockTransactionDto>>
{
    private readonly IStockTransactionRepository _repository;
    private readonly IMapper _mapper;

    public GetStockTransactionListHandler(IStockTransactionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StockTransactionDto>> Handle(GetStockTransactionListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.StockTransaction> transactions;
        
        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            transactions = await _repository.GetByDateRangeAsync(
                request.StartDate.Value, 
                request.EndDate.Value, 
                request.BranchId, 
                cancellationToken);
        }
        else if (request.TransactionTypeId.HasValue)
        {
            transactions = await _repository.GetByTypeAsync(request.TransactionTypeId.Value, cancellationToken);
        }
        else if (request.BranchId.HasValue)
        {
            transactions = await _repository.GetByBranchAsync(request.BranchId.Value, cancellationToken);
        }
        else
        {
            transactions = await _repository.GetAllAsync(cancellationToken);
        }
        
        return _mapper.Map<IEnumerable<StockTransactionDto>>(transactions);
    }
}
