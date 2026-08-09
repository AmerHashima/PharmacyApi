using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Api.Controllers;
using ECommerceIntegration.Application.Commands;
using ECommerceIntegration.Domain.Enums;

namespace Pharmacy.Api.Controllers.ECommerceIntegration;

[Route("api/ecommerce/webhooks")]
[AllowAnonymous]
public class ECommerceWebhooksController : BaseApiController
{
    private readonly IMediator _mediator;

    public ECommerceWebhooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("zid")]
    public async Task<IActionResult> ZidWebhook(CancellationToken cancellationToken)
    {
        await SaveWebhookAsync(ECommerceProviderType.Zid, cancellationToken);
        return Ok();
    }

    [HttpPost("salla")]
    public async Task<IActionResult> SallaWebhook(CancellationToken cancellationToken)
    {
        await SaveWebhookAsync(ECommerceProviderType.Salla, cancellationToken);
        return Ok();
    }

    private async Task SaveWebhookAsync(
        ECommerceProviderType providerType,
        CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body);
        var payload = await reader.ReadToEndAsync(cancellationToken);
        var eventName = Request.Headers["X-Event"].FirstOrDefault()
                        ?? Request.Headers["X-Salla-Event"].FirstOrDefault()
                        ?? Request.Headers["X-Zid-Event"].FirstOrDefault()
                        ?? "unknown";

        var externalId = Request.Headers["X-External-Id"].FirstOrDefault()
                         ?? Request.Headers["X-Order-Id"].FirstOrDefault();

        await _mediator.Send(new SaveECommerceWebhookCommand(
            providerType,
            eventName,
            externalId,
            payload), cancellationToken);
    }
}
