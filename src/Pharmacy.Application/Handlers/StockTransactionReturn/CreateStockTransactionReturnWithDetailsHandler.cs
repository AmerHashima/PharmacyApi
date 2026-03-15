using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.StockTransactionReturn;
using Pharmacy.Application.DTOs.StockTransactionReturn;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.StockTransactionReturn;

/// <summary>
/// Handler for creating a stock transaction return with detail lines.
/// </summary>
public class CreateStockTransactionReturnWithDetailsHandler
    : IRequestHandler<CreateStockTransactionReturnWithDetailsCommand, StockTransactionReturnWithDetailsDto>
{
    private readonly IStockTransactionReturnRepository _returnRepository;
    private readonly IStockTransactionReturnDetailRepository _returnDetailRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    private readonly IAppLookupDetailRepository _lookupDetailRepository;
    private readonly IMapper _mapper;

    public CreateStockTransactionReturnWithDetailsHandler(
        IStockTransactionReturnRepository returnRepository,
        IStockTransactionReturnDetailRepository returnDetailRepository,
        IBranchRepository branchRepository,
        IProductRepository productRepository,
        IAppLookupDetailRepository lookupDetailRepository,
        IMapper mapper)
    {
        _returnRepository = returnRepository;
        _returnDetailRepository = returnDetailRepository;
        _branchRepository = branchRepository;
        _productRepository = productRepository;
        _lookupDetailRepository = lookupDetailRepository;
        _mapper = mapper;
    }

    public async Task<StockTransactionReturnWithDetailsDto> Handle(
        CreateStockTransactionReturnWithDetailsCommand request,
        CancellationToken cancellationToken)
    {
        // Validate transaction type
        var transactionType = await _lookupDetailRepository.GetByIdAsync(request.Transaction.TransactionTypeId, cancellationToken)
            ?? throw new KeyNotFoundException($"Transaction type with ID '{request.Transaction.TransactionTypeId}' not found");

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

        // Create return transaction header
        var returnTransaction = new Domain.Entities.StockTransactionReturn
        {
            TransactionTypeId = request.Transaction.TransactionTypeId,
            FromBranchId = request.Transaction.FromBranchId,
            ToBranchId = request.Transaction.ToBranchId,
            ReferenceNumber = request.Transaction.ReferenceNumber,
            NotificationId = request.Transaction.NotificationId,
            TransactionDate = request.Transaction.TransactionDate,
            SupplierId = request.Transaction.SupplierId,
            Notes = request.Transaction.Notes,
            ReturnInvoiceId = request.Transaction.ReturnInvoiceId,
            OriginalTransactionId = request.Transaction.OriginalTransactionId,
            Status = request.Transaction.Status,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = null
        };

        var createdTransaction = await _returnRepository.AddAsync(returnTransaction, cancellationToken);

        // Create detail lines
        int lineNumber = 1;
        decimal totalValue = 0;

        foreach (var detailDto in request.Transaction.Details)
        {
            var detail = new StockTransactionReturnDetail
            {
                StockTransactionReturnId = createdTransaction.Oid,
                ProductId = detailDto.ProductId,
                Quantity = detailDto.Quantity,
                Gtin = detailDto.Gtin,
                BatchNumber = detailDto.BatchNumber,
                ExpiryDate = detailDto.ExpiryDate,
                SerialNumber = detailDto.SerialNumber,
                UnitCost = detailDto.UnitCost,
                TotalCost = detailDto.TotalCost ?? (detailDto.Quantity * (detailDto.UnitCost ?? 0)),
                LineNumber = detailDto.LineNumber > 0 ? detailDto.LineNumber : lineNumber,
                Notes = detailDto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            await _returnDetailRepository.AddAsync(detail, cancellationToken);
            totalValue += detail.TotalCost ?? 0;
            lineNumber++;
        }

        // Update total value on header
        createdTransaction.TotalValue = totalValue;
        await _returnRepository.UpdateAsync(createdTransaction, cancellationToken);

        return _mapper.Map<StockTransactionReturnWithDetailsDto>(createdTransaction);
    }
}
