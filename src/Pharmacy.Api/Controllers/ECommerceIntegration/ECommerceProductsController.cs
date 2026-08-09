using ECommerceIntegration.Application.Commands;
using ECommerceIntegration.Application.Dtos;
using ECommerceIntegration.Application.Queries;
using ECommerceIntegration.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Controllers;
using Pharmacy.Api.Models;
using Pharmacy.Api.Models.ECommerceIntegration;

namespace Pharmacy.Api.Controllers.ECommerceIntegration;

[Route("api/ecommerce/products")]
[Authorize]
public class ECommerceProductsController : BaseApiController
{
    private readonly IMediator _mediator;

    public ECommerceProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("pull")]
    public async Task<ActionResult<ApiResponse<ProductPullResultDto>>> PullProducts(
        [FromBody] PullProviderProductsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new PullProviderProductsCommand(request.StoreId),
            cancellationToken);

        return SuccessResponse(result, "Products pulled successfully");
    }

    [HttpPost("push-initial")]
    public async Task<ActionResult<ApiResponse<ProductPushResultDto>>> PushInitialProducts(
        [FromBody] PushInitialProductsToProviderRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new PushInitialProductsToProviderCommand(
                request.StoreId,
                request.BranchId,
                request.WarehouseId,
                request.PriceListId,
                request.BatchSize),
            cancellationToken);

        return SuccessResponse(result, "Products pushed successfully");
    }

    [HttpGet("mappings")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ECommerceProductMapping>>>> GetMappings(
        [FromQuery] Guid? storeId,
        CancellationToken cancellationToken)
    {
        var mappings = await _mediator.Send(
            new GetECommerceProductMappingsQuery(storeId),
            cancellationToken);

        return SuccessResponse<IEnumerable<ECommerceProductMapping>>(
            mappings,
            "E-Commerce product mappings retrieved successfully");
    }
}
