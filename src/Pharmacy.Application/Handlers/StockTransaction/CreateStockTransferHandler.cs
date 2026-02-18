using AutoMapper;
using Pharmacy.Application.Commands.StockTransaction;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for creating a Stock Transfer transaction (between branches)
/// </summary>
public class CreateStockTransferHandler : IRequestHandler<CreateStockTransferCommand, StockTransactionDto>
{
    private readonly IStockTransactionRepository _transactionRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IAppLookupDetailRepository _lookupRepository;
    private readonly IMapper _mapper;

    public CreateStockTransferHandler(
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

    public async Task<StockTransactionDto> Handle(CreateStockTransferCommand request, CancellationToken cancellationToken)
    {
        // Validate product exists
        var product = await _productRepository.GetByIdAsync(request.Transfer.ProductId, cancellationToken);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID '{request.Transfer.ProductId}' not found");
        }

        // Validate from branch exists
        var fromBranch = await _branchRepository.GetByIdAsync(request.Transfer.FromBranchId, cancellationToken);
        if (fromBranch == null)
        {
            throw new KeyNotFoundException($"From Branch with ID '{request.Transfer.FromBranchId}' not found");
        }

        // Validate to branch exists
        var toBranch = await _branchRepository.GetByIdAsync(request.Transfer.ToBranchId, cancellationToken);
        if (toBranch == null)
        {
            throw new KeyNotFoundException($"To Branch with ID '{request.Transfer.ToBranchId}' not found");
        }

        // Validate branches are different
        if (request.Transfer.FromBranchId == request.Transfer.ToBranchId)
        {
            throw new InvalidOperationException("Cannot transfer stock to the same branch");
        }

        // Check if sufficient stock is available
        if (!await _stockRepository.HasSufficientStockAsync(
            request.Transfer.ProductId, 
            request.Transfer.FromBranchId, 
            request.Transfer.Quantity, 
            cancellationToken))
        {
            throw new InvalidOperationException("Insufficient stock available for transfer");
        }

        // Get transaction type "TRANSFER" from lookup
        var transactionTypes = await _lookupRepository.GetByMasterCodeAsync("TRANSACTION_TYPE", cancellationToken);
        var transferType = transactionTypes.FirstOrDefault(t => t.ValueCode == "TRANSFER");

        // Generate reference number if not provided
        var referenceNumber = request.Transfer.ReferenceNumber ?? 
            await _transactionRepository.GenerateReferenceNumberAsync("TRF", cancellationToken);

        // Create transaction
        var transaction = new Domain.Entities.StockTransaction
        {
            ProductId = request.Transfer.ProductId,
            FromBranchId = request.Transfer.FromBranchId,
            ToBranchId = request.Transfer.ToBranchId,
            Quantity = request.Transfer.Quantity,
            TransactionTypeId = transferType?.Oid,
            ReferenceNumber = referenceNumber,
            TransactionDate = request.Transfer.TransactionDate ?? DateTime.UtcNow,
            BatchNumber = request.Transfer.BatchNumber,
            ExpiryDate = request.Transfer.ExpiryDate,
            Notes = request.Transfer.Notes
        };

        var createdTransaction = await _transactionRepository.AddAsync(transaction, cancellationToken);

        // Decrease stock at source branch
        await _stockRepository.UpdateQuantityAsync(
            request.Transfer.ProductId, 
            request.Transfer.FromBranchId, 
            -request.Transfer.Quantity, 
            cancellationToken);

        // Increase stock at destination branch
        await _stockRepository.UpdateQuantityAsync(
            request.Transfer.ProductId, 
            request.Transfer.ToBranchId, 
            request.Transfer.Quantity, 
            cancellationToken);

        return _mapper.Map<StockTransactionDto>(createdTransaction);
    }
}
