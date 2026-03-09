using MediatR;
using Pharmacy.Application.Commands.Rsd;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Interfaces;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Rsd;

public class GetDispatchDetailHandler : IRequestHandler<GetDispatchDetailCommand, DispatchDetailResponseDto>
{
    private readonly IRsdIntegrationService _rsdService;
    private readonly IProductRepository _productRepository;
    private readonly IStakeholderRepository _stakeholderRepository;

    public GetDispatchDetailHandler(
        IRsdIntegrationService rsdService,
        IProductRepository productRepository,
        IStakeholderRepository stakeholderRepository)
    {
        _rsdService = rsdService;
        _productRepository = productRepository;
        _stakeholderRepository = stakeholderRepository;
    }

    public async Task<DispatchDetailResponseDto> Handle(GetDispatchDetailCommand request, CancellationToken cancellationToken)
    {
        var result = await _rsdService.GetDispatchDetailAsync(request.Request, cancellationToken);

        if (!result.Success)
            return result;

        // Match supplier by FromGLN
        if (!string.IsNullOrEmpty(result.FromGLN))
        {
            var supplier = await _stakeholderRepository.GetByGLNAsync(result.FromGLN, cancellationToken);
            if (supplier != null)
            {
                result.SupplierId = supplier.Oid;
                result.SupplierName = supplier.Name;
            }
        }

        // Match products by GTIN
        foreach (var product in result.Products)
        {
            if (string.IsNullOrEmpty(product.GTIN))
                continue;

            var matchedProduct = await _productRepository.GetByGTINAsync(product.GTIN, cancellationToken);
            if (matchedProduct != null)
            {
                product.ProductId = matchedProduct.Oid;
                product.ProductName = matchedProduct.DrugName;
            }
        }

        return result;
    }
}