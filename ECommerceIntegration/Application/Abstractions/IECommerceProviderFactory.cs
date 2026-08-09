using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Application.Abstractions;

public interface IECommerceProviderFactory
{
    IECommerceProvider GetProvider(ECommerceProviderType providerType);
}
