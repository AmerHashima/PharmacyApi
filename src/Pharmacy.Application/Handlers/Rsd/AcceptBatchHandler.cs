using MediatR;
using Pharmacy.Application.Commands.Rsd;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Rsd;

public class AcceptBatchHandler : IRequestHandler<AcceptBatchCommand, AcceptBatchResponseDto>
{
    /// <summary>RSD_OPERATION_TYPE → ACCEPT_BATCH</summary>
    private static readonly Guid OperationTypeAcceptBatch = Guid.Parse("22222222-2222-2222-2222-2222222220B0");
    private readonly IRsdIntegrationService _rsdService;
    private readonly IRsdOperationLogRepository _logRepository;
    private readonly IProductRepository _productRepository;

    public AcceptBatchHandler(
        IRsdIntegrationService rsdService,
        IRsdOperationLogRepository logRepository,
        IProductRepository productRepository)
    {
        _rsdService = rsdService;
        _logRepository = logRepository;
        _productRepository = productRepository;
    }

    public async Task<AcceptBatchResponseDto> Handle(AcceptBatchCommand request, CancellationToken cancellationToken)
    {
        var result = await _rsdService.AcceptBatchAsync(request.Request, cancellationToken);

        var log = new RsdOperationLog
        {
            OperationTypeId = OperationTypeAcceptBatch,
            BranchId = request.Request.BranchId,
            GLN = request.Request.FromGLN,
            NotificationId = result.NotificationId,
            Success = result.Success,
            ResponseCode = result.ResponseCode,
            ResponseMessage = result.ResponseMessage,
            RawResponse = result.RawResponse
        };

        foreach (var product in result.Products)
        {
            var matched = await _productRepository.GetByGTINAsync(product.GTIN, cancellationToken);
            log.Details.Add(new RsdOperationLogDetail
            {
                GTIN = product.GTIN,
                ProductId = matched?.Oid,
                BatchNumber = product.BatchNumber,
                ExpiryDate = product.ExpiryDate,
                Quantity = product.Quantity,
                ResponseCode = product.ResponseCode
            });
        }

        await _logRepository.AddAsync(log, cancellationToken);

        return result;
    }
}