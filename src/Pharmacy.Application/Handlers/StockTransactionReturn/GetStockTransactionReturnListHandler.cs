using AutoMapper;
using Pharmacy.Application.DTOs.StockTransactionReturn;
using Pharmacy.Application.Queries.StockTransactionReturn;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.StockTransactionReturn;

/// <summary>
/// Handler for getting stock transaction returns list
/// </summary>
public class GetStockTransactionReturnListHandler : IRequestHandler<GetStockTransactionReturnListQuery, IEnumerable<StockTransactionReturnDto>>
{
    private readonly IStockTransactionReturnRepository _repository;
    private readonly IMapper _mapper;

    public GetStockTransactionReturnListHandler(IStockTransactionReturnRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StockTransactionReturnDto>> Handle(GetStockTransactionReturnListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.StockTransactionReturn> transactions;

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

        return _mapper.Map<IEnumerable<StockTransactionReturnDto>>(transactions);
    }
}
