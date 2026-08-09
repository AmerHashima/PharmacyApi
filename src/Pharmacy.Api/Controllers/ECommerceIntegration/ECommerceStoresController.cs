using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Pharmacy.Api.Controllers;
using Pharmacy.Api.Models;
using Pharmacy.Api.Models.ECommerceIntegration;
using ECommerceIntegration.Application.Commands;
using ECommerceIntegration.Application.Queries;
using ECommerceIntegration.Domain.Entities;
using ECommerceIntegration.Domain.Enums;

namespace Pharmacy.Api.Controllers.ECommerceIntegration;

[Route("api/ecommerce/stores")]
[Authorize]
public class ECommerceStoresController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public ECommerceStoresController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ECommerceStore>>>> GetStores(
        CancellationToken cancellationToken)
    {
        var stores = await _mediator.Send(new GetECommerceStoreListQuery(), cancellationToken);
        return SuccessResponse<IEnumerable<ECommerceStore>>(stores, "E-Commerce stores retrieved successfully");
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<ECommerceStore>>> GetStore(
        Guid id,
        CancellationToken cancellationToken)
    {
        var store = await _mediator.Send(new GetECommerceStoreByIdQuery(id), cancellationToken);

        if (store == null)
        {
            return ErrorResponse<ECommerceStore>("E-Commerce store not found", 404);
        }

        return SuccessResponse(store, "E-Commerce store retrieved successfully");
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ECommerceStore>>> CreateStore(
        [FromBody] CreateECommerceStoreRequest request,
        CancellationToken cancellationToken)
    {
        var store = await _mediator.Send(new CreateECommerceStoreCommand(
            request.ProviderType,
            request.ExternalStoreId,
            request.StoreName,
            request.AccessToken,
            request.RefreshToken,
            request.TokenExpiresAt,
            request.BranchId,
            request.WarehouseId,
            request.PriceListId,
            request.AutoPullOrders,
            request.AutoSyncStock,
            request.AutoSyncPrice,
            request.IsActive), cancellationToken);

        return CreatedResponse(store, nameof(GetStore), new { id = store.Oid }, "E-Commerce store created successfully");
    }

    [HttpPost("connect")]
    public async Task<ActionResult<ApiResponse<ECommerceStore>>> ConnectStore(
        [FromBody] ConnectECommerceStoreRequest request,
        CancellationToken cancellationToken)
    {
        var store = await _mediator.Send(new ConnectECommerceStoreCommand(
            request.ProviderType,
            request.Code,
            request.BranchId,
            request.WarehouseId,
            request.PriceListId), cancellationToken);

        return SuccessResponse(store, "E-Commerce store connected successfully");
    }

    [HttpGet("connect/{ecommerceApp}/redirect")]
    [AllowAnonymous]
    public ActionResult<ApiResponse> ConnectStoreRedirect(
        string ecommerceApp,
        [FromQuery] string? state,
        CancellationToken cancellationToken)
    {
        if (!TryResolveProviderType(ecommerceApp, out var providerType))
        {
            return ErrorResponse($"Unsupported e-commerce app '{ecommerceApp}'.", 400);
        }

        if (!TryBuildAuthorizationUrl(providerType, state, out var authorizationUrl, out var error))
        {
            return ErrorResponse(error, 500);
        }

        return Redirect(authorizationUrl);
    }

    [HttpGet("connect/{ecommerceApp}/callback")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<ECommerceStore>>> ConnectStoreCallback(
        string ecommerceApp,
        [FromQuery] string? code,
        [FromQuery] string? state,
        [FromQuery] Guid? branchId,
        [FromQuery] Guid? warehouseId,
        [FromQuery] Guid? priceListId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return ErrorResponse<ECommerceStore>("Authorization code is required.", 400);
        }

        if (!TryResolveProviderType(ecommerceApp, out var providerType))
        {
            return ErrorResponse<ECommerceStore>($"Unsupported e-commerce app '{ecommerceApp}'.", 400);
        }

        ECommerceStore store;
        try
        {
            store = await _mediator.Send(new ConnectECommerceStoreCommand(
                providerType,
                code,
                branchId,
                warehouseId,
                priceListId), cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<ECommerceStore>(ex.Message, 400);
        }

        return SuccessResponse(store, "E-Commerce store connected successfully");
    }

    [HttpPut("{id:guid}/settings")]
    public async Task<ActionResult<ApiResponse<ECommerceStore>>> UpdateSettings(
        Guid id,
        [FromBody] UpdateECommerceStoreSettingsRequest request,
        CancellationToken cancellationToken)
    {
        var store = await _mediator.Send(new UpdateECommerceStoreSettingsCommand(
            id,
            request.BranchId,
            request.WarehouseId,
            request.PriceListId,
            request.AutoPullOrders,
            request.AutoSyncStock,
            request.AutoSyncPrice,
            request.IsActive), cancellationToken);

        if (store == null)
        {
            return ErrorResponse<ECommerceStore>("E-Commerce store not found", 404);
        }

        return SuccessResponse(store, "E-Commerce store settings updated successfully");
    }

    private static bool TryResolveProviderType(string ecommerceApp, out ECommerceProviderType providerType)
    {
        return Enum.TryParse(ecommerceApp, ignoreCase: true, out providerType);
    }

    private bool TryBuildAuthorizationUrl(
        ECommerceProviderType providerType,
        string? state,
        out string authorizationUrl,
        out string error)
    {
        authorizationUrl = string.Empty;
        error = string.Empty;

        if (providerType != ECommerceProviderType.Zid)
        {
            error = $"Redirect flow is not configured for provider '{providerType}'.";
            return false;
        }

        var section = _configuration.GetSection($"ECommerceIntegration:{providerType}");
        var clientId = section["ClientId"];
        var redirectUri = section["RedirectUri"];
        var authorizeUrl = section["AuthorizeUrl"] ?? section["AuthorizationUrl"];
        if (string.IsNullOrWhiteSpace(authorizeUrl))
        {
            authorizeUrl = "https://oauth.zid.sa/oauth/authorize";
        }

        if (string.IsNullOrWhiteSpace(clientId))
        {
            error = $"E-Commerce provider '{providerType}' ClientId is not configured.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(redirectUri))
        {
            error = $"E-Commerce provider '{providerType}' RedirectUri is not configured.";
            return false;
        }

        var query = new Dictionary<string, string?>
        {
            ["client_id"] = clientId,
            ["redirect_uri"] = redirectUri,
            ["response_type"] = "code"
        };

        if (!string.IsNullOrWhiteSpace(state))
        {
            query["state"] = state;
        }

        authorizationUrl = QueryHelpers.AddQueryString(authorizeUrl, query);
        return true;
    }
}
