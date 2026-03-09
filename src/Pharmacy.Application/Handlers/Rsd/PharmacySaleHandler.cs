using MediatR;
using Pharmacy.Application.Commands.Rsd;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Rsd;

public class PharmacySaleHandler : IRequestHandler<PharmacySaleCommand, PharmacySaleResponseDto>
{
    /// <summary>RSD_OPERATION_TYPE → PHARMACY_SALE</summary>
    private static readonly Guid OperationTypePharmacySale = Guid.Parse("22222222-2222-2222-2222-2222222220B1");
    private readonly IRsdIntegrationService _rsdService;
    private readonly IRsdOperationLogRepository _logRepository;
    private readonly IProductRepository _productRepository;

    public PharmacySaleHandler(
        IRsdIntegrationService rsdService,
        IRsdOperationLogRepository logRepository,
        IProductRepository productRepository)
    {
        _rsdService = rsdService;
        _logRepository = logRepository;
        _productRepository = productRepository;
    }

    public async Task<PharmacySaleResponseDto> Handle(PharmacySaleCommand request, CancellationToken cancellationToken)
    {
        var result = await _rsdService.PharmacySaleAsync(request.Request, cancellationToken);

        var log = new RsdOperationLog
        {
            OperationTypeId = OperationTypePharmacySale,
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
                SerialNumber = product.SerialNumber,
                ResponseCode = product.ResponseCode
            });
        }

        await _logRepository.AddAsync(log, cancellationToken);

        return result;
    }
}