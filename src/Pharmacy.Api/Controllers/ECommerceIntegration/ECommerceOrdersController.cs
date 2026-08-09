using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Controllers;
using Pharmacy.Api.Models;
using Pharmacy.Api.Models.ECommerceIntegration;
using ECommerceIntegration.Application.Commands;
using ECommerceIntegration.Application.Queries;
using ECommerceIntegration.Domain.Entities;

namespace Pharmacy.Api.Controllers.ECommerceIntegration;

[Route("api/ecommerce/orders")]
[Authorize]
public class ECommerceOrdersController : BaseApiController
{
    private readonly IMediator _mediator;

    public ECommerceOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ECommerceOrder>>>> GetOrders(
        [FromQuery] Guid? storeId,
        CancellationToken cancellationToken)
    {
        var orders = await _mediator.Send(new GetECommerceOrderListQuery(storeId), cancellationToken);
        return SuccessResponse<IEnumerable<ECommerceOrder>>(orders, "E-Commerce orders retrieved successfully");
    }

    [HttpPost("pull")]
    public async Task<ActionResult<ApiResponse>> PullOrders(
        [FromBody] PullOrdersRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new PullECommerceOrdersCommand(
            request.StoreId,
            request.FromDate,
            request.ToDate), cancellationToken);

        return SuccessResponse("Orders pulled successfully");
    }

    [HttpPost("pull/{externalOrderId}")]
    public async Task<ActionResult<ApiResponse>> PullOrderByExternalId(
        string externalOrderId,
        [FromBody] PullOrderRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new PullECommerceOrderByExternalIdCommand(
            request.StoreId,
            externalOrderId), cancellationToken);

        return SuccessResponse("Order pulled successfully");
    }
}
