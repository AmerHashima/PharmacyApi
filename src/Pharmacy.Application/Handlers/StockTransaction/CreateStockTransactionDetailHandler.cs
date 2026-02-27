using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.StockTransaction;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for creating a new stock transaction detail
/// </summary>
public class CreateStockTransactionDetailHandler : IRequestHandler<CreateStockTransactionDetailCommand, StockTransactionDetailDto>
{
    private readonly IStockTransactionDetailRepository _repository;
    private readonly IStockTransactionRepository _transactionRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateStockTransactionDetailHandler(
        IStockTransactionDetailRepository repository,
        IStockTransactionRepository transactionRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _repository = repository;
        _transactionRepository = transactionRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<StockTransactionDetailDto> Handle(CreateStockTransactionDetailCommand request, CancellationToken cancellationToken)
    {
        // Validate stock transaction exists
        var transaction = await _transactionRepository.GetByIdAsync(request.Detail.StockTransactionId, cancellationToken);
        if (transaction == null)
        {
            throw new KeyNotFoundException($"Stock transaction with ID '{request.Detail.StockTransactionId}' not found");
        }

        // Validate product exists
        var product = await _productRepository.GetByIdAsync(request.Detail.ProductId, cancellationToken);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID '{request.Detail.ProductId}' not found");
        }

        // Map DTO to entity
        var detail = _mapper.Map<StockTransactionDetail>(request.Detail);

        // Set audit fields
        detail.CreatedAt = DateTime.UtcNow;
        detail.CreatedBy = null; // TODO: Get from current user context

        // Add to repository
        await _repository.AddAsync(detail, cancellationToken);

        // Map back to DTO and return
        var result = _mapper.Map<StockTransactionDetailDto>(detail);
        result.ProductName = product.DrugName;
        result.ProductGTIN = product.GTIN;

        return result;
    }
}
