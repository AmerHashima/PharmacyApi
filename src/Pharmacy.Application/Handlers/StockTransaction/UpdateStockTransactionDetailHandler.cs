using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.StockTransaction;
using Pharmacy.Application.DTOs.StockTransaction;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.StockTransaction;

/// <summary>
/// Handler for updating a stock transaction detail
/// </summary>
public class UpdateStockTransactionDetailHandler : IRequestHandler<UpdateStockTransactionDetailCommand, StockTransactionDetailDto>
{
    private readonly IStockTransactionDetailRepository _repository;
    private readonly IMapper _mapper;

    public UpdateStockTransactionDetailHandler(
        IStockTransactionDetailRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StockTransactionDetailDto> Handle(UpdateStockTransactionDetailCommand request, CancellationToken cancellationToken)
    {
        // Get existing entity
        var existingDetail = await _repository.GetByIdAsync(request.Detail.Oid, cancellationToken);
        if (existingDetail == null)
        {
            throw new KeyNotFoundException($"Stock transaction detail with ID '{request.Detail.Oid}' not found");
        }

        // Map updated values to existing entity
        _mapper.Map(request.Detail, existingDetail);

        // Set audit fields
        existingDetail.UpdatedAt = DateTime.UtcNow;
        existingDetail.UpdatedBy = null; // TODO: Get from current user context

        // Update in repository
        await _repository.UpdateAsync(existingDetail, cancellationToken);

        // Map back to DTO and return
        return _mapper.Map<StockTransactionDetailDto>(existingDetail);
    }
}
