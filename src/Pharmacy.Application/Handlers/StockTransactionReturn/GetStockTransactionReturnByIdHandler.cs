using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.StockTransactionReturn;
using Pharmacy.Application.Queries.StockTransactionReturn;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.StockTransactionReturn;

/// <summary>
/// Handler for getting stock transaction return by ID with details
/// </summary>
public class GetStockTransactionReturnByIdHandler : IRequestHandler<GetStockTransactionReturnByIdQuery, StockTransactionReturnWithDetailsDto?>
{
    private readonly IStockTransactionReturnRepository _repository;
    private readonly IMapper _mapper;

    public GetStockTransactionReturnByIdHandler(
        IStockTransactionReturnRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StockTransactionReturnWithDetailsDto?> Handle(GetStockTransactionReturnByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _repository.GetQueryable()
            .Include(x => x.FromBranch)
            .Include(x => x.ToBranch)
            .Include(x => x.TransactionType)
            .Include(x => x.Supplier)
            .Include(x => x.ReturnInvoice)
            .Include(x => x.OriginalTransaction)
            .Include(x => x.Details)
                .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(x => x.Oid == request.Id && !x.IsDeleted, cancellationToken);

        if (transaction == null)
            return null;

        return _mapper.Map<StockTransactionReturnWithDetailsDto>(transaction);
    }
}
