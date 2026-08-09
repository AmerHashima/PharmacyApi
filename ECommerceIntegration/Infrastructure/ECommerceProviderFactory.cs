using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Infrastructure;

public class ECommerceProviderFactory : IECommerceProviderFactory
{
    private readonly IEnumerable<IECommerceProvider> _providers;

    public ECommerceProviderFactory(IEnumerable<IECommerceProvider> providers)
    {
        _providers = providers;
    }

    public IECommerceProvider GetProvider(ECommerceProviderType providerType)
    {
        var provider = _providers.FirstOrDefault(x => x.ProviderType == providerType);
        if (provider == null)
        {
            throw new NotSupportedException($"Provider {providerType} is not supported.");
        }

        return provider;
    }
}
