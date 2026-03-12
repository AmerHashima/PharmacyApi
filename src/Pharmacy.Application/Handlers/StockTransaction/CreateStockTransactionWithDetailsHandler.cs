using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.StockTransaction;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for creating a stock transaction with detail lines.
/// Automatically updates Stock table based on transaction type.
/// </summary>
public class CreateStockTransactionWithDetailsHandler 
    : IRequestHandler<CreateStockTransactionWithDetailsCommand, StockTransactionWithDetailsDto>
{
    private readonly IStockTransactionRepository _transactionRepository;
    private readonly IStockTransactionDetailRepository _StockTransactionDetail;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IAppLookupDetailRepository _lookupDetailRepository;
    private readonly IMapper _mapper;

    public CreateStockTransactionWithDetailsHandler(
        IStockTransactionRepository transactionRepository,
        IStockTransactionDetailRepository StockTransactionDetail,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        IStockRepository stockRepository,
        IAppLookupDetailRepository lookupDetailRepository,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _StockTransactionDetail = StockTransactionDetail;
        _branchRepository = branchRepository;
        _productRepository = productRepository;
        _stockRepository = stockRepository;
        _lookupDetailRepository = lookupDetailRepository;
        _mapper = mapper;
    }

    public async Task<StockTransactionWithDetailsDto> Handle(
        CreateStockTransactionWithDetailsCommand request, 
        CancellationToken cancellationToken)
    {
        // Resolve the transaction type ValueCode
        var transactionType = await _lookupDetailRepository.GetByIdAsync(request.Transaction.TransactionTypeId, cancellationToken)
            ?? throw new KeyNotFoundException($"Transaction type with ID '{request.Transaction.TransactionTypeId}' not found");

        var typeCode = transactionType.ValueCode?.ToUpperInvariant();

        // Validate branches based on transaction type
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

        // Validate required branches per transaction type
        ValidateBranches(typeCode, request.Transaction.FromBranchId, request.Transaction.ToBranchId);

        // Validate all products exist
        foreach (var detailDto in request.Transaction.Details)
        {
            var product = await _productRepository.GetByIdAsync(detailDto.ProductId, cancellationToken);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID '{detailDto.ProductId}' not found");
        }

        // For OUT/TRANSFER/EXPIRED/DAMAGED — check sufficient stock
        if (typeCode is "OUT" or "TRANSFER" or "EXPIRED" or "DAMAGED")
        {
            var sourceBranchId = request.Transaction.FromBranchId
                ?? throw new InvalidOperationException($"FromBranchId is required for {typeCode} transactions");

            foreach (var detailDto in request.Transaction.Details)
            {
                if (!await _stockRepository.HasSufficientStockAsync(detailDto.ProductId, sourceBranchId, detailDto.Quantity, detailDto.BatchNumber, cancellationToken))
                    throw new InvalidOperationException($"Insufficient stock for product '{detailDto.ProductId}' at branch '{sourceBranchId}'");
            }
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
            CreatedBy = null
        };

        var createdTransaction = await _transactionRepository.AddAsync(transaction, cancellationToken);

        // Create detail lines and update stock
        int lineNumber = 1;
        decimal totalValue = 0;

        foreach (var detailDto in request.Transaction.Details)
        {
            var detail = new StockTransactionDetail
            {
                StockTransactionId = createdTransaction.Oid,
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

            // Update stock based on transaction type
            await UpdateStockAsync(typeCode, detailDto.ProductId, detailDto.Quantity,
                request.Transaction.FromBranchId, request.Transaction.ToBranchId,
                detailDto.BatchNumber, detailDto.ExpiryDate, cancellationToken);
        }

        // Update total value on master
        createdTransaction.TotalValue = totalValue;

        // Fetch complete transaction with all includes
        var completeTransaction = await _transactionRepository.GetByIdAsync(createdTransaction.Oid, cancellationToken);

        return _mapper.Map<StockTransactionWithDetailsDto>(completeTransaction);
    }

    /// <summary>
    /// Updates stock quantities based on transaction type:
    /// IN      → +quantity at ToBranch
    /// OUT     → −quantity at FromBranch
    /// TRANSFER→ −quantity at FromBranch, +quantity at ToBranch
    /// RETURN  → +quantity at ToBranch
    /// ADJUSTMENT → +quantity at ToBranch (use negative quantity for decrease)
    /// EXPIRED/DAMAGED → −quantity at FromBranch
    /// </summary>
    private async Task UpdateStockAsync(string? typeCode, Guid productId, decimal quantity,
        Guid? fromBranchId, Guid? toBranchId, string? batchNumber, DateTime? expiryDate,
        CancellationToken cancellationToken)
    {
        switch (typeCode)
        {
            case "IN":
            case "RETURN":
                if (toBranchId.HasValue)
                    await _stockRepository.UpdateQuantityAsync(productId, toBranchId.Value, quantity, batchNumber, expiryDate, cancellationToken);
                break;

            case "OUT":
            case "EXPIRED":
            case "DAMAGED":
                if (fromBranchId.HasValue)
                    await _stockRepository.UpdateQuantityAsync(productId, fromBranchId.Value, -quantity, batchNumber, expiryDate, cancellationToken);
                break;

            case "TRANSFER":
                if (fromBranchId.HasValue)
                    await _stockRepository.UpdateQuantityAsync(productId, fromBranchId.Value, -quantity, batchNumber, expiryDate, cancellationToken);
                if (toBranchId.HasValue)
                    await _stockRepository.UpdateQuantityAsync(productId, toBranchId.Value, quantity, batchNumber, expiryDate, cancellationToken);
                break;

            case "ADJUSTMENT":
                if (toBranchId.HasValue)
                    await _stockRepository.UpdateQuantityAsync(productId, toBranchId.Value, quantity, batchNumber, expiryDate, cancellationToken);
                else if (fromBranchId.HasValue)
                    await _stockRepository.UpdateQuantityAsync(productId, fromBranchId.Value, quantity, batchNumber, expiryDate, cancellationToken);
                break;
        }
    }

    private static void ValidateBranches(string? typeCode, Guid? fromBranchId, Guid? toBranchId)
    {
        switch (typeCode)
        {
            case "IN":
            case "RETURN":
                if (!toBranchId.HasValue)
                    throw new InvalidOperationException($"ToBranchId is required for {typeCode} transactions");
                break;

            case "OUT":
            case "EXPIRED":
            case "DAMAGED":
                if (!fromBranchId.HasValue)
                    throw new InvalidOperationException($"FromBranchId is required for {typeCode} transactions");
                break;

            case "TRANSFER":
                if (!fromBranchId.HasValue || !toBranchId.HasValue)
                    throw new InvalidOperationException("Both FromBranchId and ToBranchId are required for TRANSFER transactions");
                if (fromBranchId == toBranchId)
                    throw new InvalidOperationException("Cannot transfer stock to the same branch");
                break;
        }
    }
}
