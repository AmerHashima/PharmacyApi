using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        var transaction = await _repository.GetQueryable()
            .Include(x => x.FromBranch)
            .Include(x => x.ToBranch)
            .Include(x => x.TransactionType)
            .Include(x => x.Supplier)
            .Include(x => x.Details)
                .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(x => x.Oid == request.Id && !x.IsDeleted, cancellationToken);

        if (transaction == null)
            return null;

        return _mapper.Map<StockTransactionWithDetailsDto>(transaction);
    }
}
