using MediatR;
using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Application.Commands;

public record SaveECommerceWebhookCommand(
    ECommerceProviderType ProviderType,
    string EventName,
    string? ExternalId,
    string Payload) : IRequest<Unit>;
