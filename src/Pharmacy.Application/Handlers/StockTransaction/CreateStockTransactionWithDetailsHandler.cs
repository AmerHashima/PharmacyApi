using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.StockTransaction;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for creating a stock transaction with detail lines
/// </summary>
public class CreateStockTransactionWithDetailsHandler 
    : IRequestHandler<CreateStockTransactionWithDetailsCommand, StockTransactionWithDetailsDto>
{
    private readonly IStockTransactionRepository _transactionRepository;
    private readonly IStockTransactionDetailRepository _StockTransactionDetail;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateStockTransactionWithDetailsHandler(
        IStockTransactionRepository transactionRepository,
        IStockTransactionDetailRepository StockTransactionDetail,

    IBranchRepository branchRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _StockTransactionDetail = StockTransactionDetail;
        _branchRepository = branchRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<StockTransactionWithDetailsDto> Handle(
        CreateStockTransactionWithDetailsCommand request, 
        CancellationToken cancellationToken)
    {
        // Validate branches
        if (request.Transaction.FromBranchId.HasValue)
        {
            var fromBranch = await _branchRepository.GetByIdAsync(request.Transaction.FromBranchId.Value, cancellationToken);
            if (fromBranch == null)
                throw new KeyNotFoundException($"From Branch with ID '{request.Transaction.FromBranchId}' not found");
        }

        if (request.Transaction.ToBranchId.HasValue)
        {
            var toBranch = await _branchRepository.GetByIdAsync(request.Transaction.ToBranchId.Value, cancellationToken);
            if (toBranch == null)
                throw new KeyNotFoundException($"To Branch with ID '{request.Transaction.ToBranchId}' not found");
        }

        // Validate all products exist
        foreach (var detailDto in request.Transaction.Details)
        {
            var product = await _productRepository.GetByIdAsync(detailDto.ProductId, cancellationToken);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID '{detailDto.ProductId}' not found");
        }

        // Create transaction header
        var transaction = new Domain.Entities.StockTransaction
        {
            TransactionTypeId = request.Transaction.TransactionTypeId,
            FromBranchId = request.Transaction.FromBranchId,
            ToBranchId = request.Transaction.ToBranchId,
            ReferenceNumber = request.Transaction.ReferenceNumber,
            NotificationId = request.Transaction.NotificationId,
            TransactionDate = request.Transaction.TransactionDate,
            SupplierId = request.Transaction.SupplierId,
            Notes = request.Transaction.Notes,
            Status = request.Transaction.Status,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = null // TODO: Get from current user context
        };

        // First, save the master transaction to get the ID
        var createdTransaction = await _transactionRepository.AddAsync(transaction, cancellationToken);

        // Now create detail lines with the transaction ID
        int lineNumber = 1;
        decimal totalValue = 0;

        foreach (var detailDto in request.Transaction.Details)
        {
            var detail = new StockTransactionDetail
            {
                StockTransactionId = createdTransaction.Oid, // Set FK from saved master
                ProductId = detailDto.ProductId,
                Quantity = detailDto.Quantity,
                Gtin = detailDto.Gtin,
                BatchNumber = detailDto.BatchNumber,
                ExpiryDate = detailDto.ExpiryDate,
                SerialNumber = detailDto.SerialNumber,
                UnitCost = detailDto.UnitCost,
                TotalCost = detailDto.TotalCost ?? (detailDto.Quantity * (detailDto.UnitCost ?? 0)),
                LineNumber = lineNumber++,
                Notes = detailDto.Notes,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = null
            };

            createdTransaction.Details.Add(detail);
            await _StockTransactionDetail.AddAsync(detail, cancellationToken);

            totalValue += detail.TotalCost ?? 0;
        }

        // Update total value on master
        createdTransaction.TotalValue = totalValue;

        // Fetch complete transaction with all includes
        var completeTransaction = await _transactionRepository.GetByIdAsync(createdTransaction.Oid, cancellationToken);

        // Map to DTO
        return _mapper.Map<StockTransactionWithDetailsDto>(completeTransaction);
    }
}
