using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.StockTransaction;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for updating a stock transaction with detail lines
/// </summary>
public class UpdateStockTransactionWithDetailsHandler 
    : IRequestHandler<UpdateStockTransactionWithDetailsCommand, StockTransactionWithDetailsDto>
{
    private readonly IStockTransactionRepository _transactionRepository;
    private readonly IStockTransactionDetailRepository _detailRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateStockTransactionWithDetailsHandler(
        IStockTransactionRepository transactionRepository,
        IStockTransactionDetailRepository detailRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _detailRepository = detailRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<StockTransactionWithDetailsDto> Handle(
        UpdateStockTransactionWithDetailsCommand request, 
        CancellationToken cancellationToken)
    {
        // Get existing transaction
        var existingTransaction = await _transactionRepository.GetByIdAsync(request.Transaction.Oid, cancellationToken);
        if (existingTransaction == null)
            throw new KeyNotFoundException($"Stock transaction with ID '{request.Transaction.Oid}' not found");

        // Validate all products exist
        foreach (var detailDto in request.Transaction.Details)
        {
            var product = await _productRepository.GetByIdAsync(detailDto.ProductId, cancellationToken);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID '{detailDto.ProductId}' not found");
        }

        // Update header fields
        existingTransaction.TransactionTypeId = request.Transaction.TransactionTypeId;
        existingTransaction.FromBranchId = request.Transaction.FromBranchId;
        existingTransaction.ToBranchId = request.Transaction.ToBranchId;
        existingTransaction.ReferenceNumber = request.Transaction.ReferenceNumber;
        existingTransaction.NotificationId = request.Transaction.NotificationId;
        existingTransaction.TransactionDate = request.Transaction.TransactionDate;
        existingTransaction.SupplierId = request.Transaction.SupplierId;
        existingTransaction.Notes = request.Transaction.Notes;
        existingTransaction.Status = request.Transaction.Status;
        existingTransaction.UpdatedAt = DateTime.UtcNow;
        existingTransaction.UpdatedBy = null; // TODO: Get from current user context

        // Delete existing details
        await _detailRepository.DeleteByTransactionIdAsync(request.Transaction.Oid, cancellationToken);

        // Create new detail lines
        existingTransaction.Details.Clear();
        int lineNumber = 1;
        decimal totalValue = 0;

        foreach (var detailDto in request.Transaction.Details)
        {
            var detail = new StockTransactionDetail
            {
                StockTransactionId = existingTransaction.Oid,
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

            existingTransaction.Details.Add(detail);
            totalValue += detail.TotalCost ?? 0;
        }

        // Update total value
        existingTransaction.TotalValue = totalValue;

        // Save changes
        await _transactionRepository.UpdateAsync(existingTransaction, cancellationToken);

        // Fetch complete transaction with includes
        var completeTransaction = await _transactionRepository.GetByIdAsync(existingTransaction.Oid, cancellationToken);

        // Map to DTO
        return _mapper.Map<StockTransactionWithDetailsDto>(completeTransaction);
    }
}
