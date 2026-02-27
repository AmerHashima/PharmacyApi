using AutoMapper;
using Pharmacy.Application.Commands.StockTransaction;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for creating a Stock IN transaction (receiving inventory from supplier)
/// </summary>
public class CreateStockInHandler : IRequestHandler<CreateStockInCommand, StockTransactionDto>
{
    private readonly IStockTransactionRepository _transactionRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IAppLookupDetailRepository _lookupRepository;
    private readonly IMapper _mapper;

    public CreateStockInHandler(
        IStockTransactionRepository transactionRepository,
        IStockRepository stockRepository,
        IProductRepository productRepository,
        IBranchRepository branchRepository,
        IAppLookupDetailRepository lookupRepository,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _stockRepository = stockRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _lookupRepository = lookupRepository;
        _mapper = mapper;
    }

    public async Task<StockTransactionDto> Handle(CreateStockInCommand request, CancellationToken cancellationToken)
    {
        // Validate product exists
        var product = await _productRepository.GetByIdAsync(request.StockIn.ProductId, cancellationToken);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID '{request.StockIn.ProductId}' not found");
        }

        // Validate branch exists
        var branch = await _branchRepository.GetByIdAsync(request.StockIn.ToBranchId, cancellationToken);
        if (branch == null)
        {
            throw new KeyNotFoundException($"Branch with ID '{request.StockIn.ToBranchId}' not found");
        }

        // Get transaction type "IN" from lookup
        var transactionTypes = await _lookupRepository.GetByMasterCodeAsync("TRANSACTION_TYPE", cancellationToken);
        var inType = transactionTypes.FirstOrDefault(t => t.ValueCode == "IN");

        // Generate reference number if not provided
        var referenceNumber = request.StockIn.ReferenceNumber ?? 
            await _transactionRepository.GenerateReferenceNumberAsync("STK-IN", cancellationToken);

        // Create transaction header
        var transaction = new Domain.Entities.StockTransaction
        {
            ToBranchId = request.StockIn.ToBranchId,
            FromBranchId = null,
            TransactionTypeId = inType?.Oid,
            ReferenceNumber = referenceNumber,
            TransactionDate = request.StockIn.TransactionDate ?? DateTime.UtcNow,
            TotalValue = request.StockIn.Quantity * (request.StockIn.UnitCost ?? 0),
            SupplierId = request.StockIn.SupplierId,
            Status = "Completed",
            Notes = request.StockIn.Notes
        };

        // Create detail line
        var transactionDetail = new Domain.Entities.StockTransactionDetail
        {
            ProductId = request.StockIn.ProductId,
            Quantity = request.StockIn.Quantity,
            UnitCost = request.StockIn.UnitCost,
            TotalCost = request.StockIn.Quantity * (request.StockIn.UnitCost ?? 0),
            BatchNumber = request.StockIn.BatchNumber,
            ExpiryDate = request.StockIn.ExpiryDate,
            LineNumber = 1
        };

        transaction.Details.Add(transactionDetail);

        var createdTransaction = await _transactionRepository.AddAsync(transaction, cancellationToken);

        // Update stock
        await _stockRepository.UpdateQuantityAsync(
            request.StockIn.ProductId, 
            request.StockIn.ToBranchId, 
            request.StockIn.Quantity, 
            cancellationToken);

        return _mapper.Map<StockTransactionDto>(createdTransaction);
    }
}
