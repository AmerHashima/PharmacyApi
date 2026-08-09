using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Application.Services;
using ECommerceIntegration.Infrastructure.Providers.Salla;
using ECommerceIntegration.Infrastructure.Providers.Zid;

namespace ECommerceIntegration.Infrastructure;

public static class ECommerceIntegrationDependencyInjection
{
    public static IServiceCollection AddECommerceIntegration(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ECommerceIntegrationDependencyInjection).Assembly));

        services.AddScoped<IECommerceProviderFactory, ECommerceProviderFactory>();
        services.AddScoped<IECommerceOrderSyncService, ECommerceOrderSyncService>();
        services.AddScoped<IECommerceProductMappingService, ECommerceProductMappingService>();
        services.AddScoped<IECommerceProductSyncService, ECommerceProductSyncService>();
        services.AddScoped<ECommerceWebhookProcessor>();
        services.TryAddScoped<ILocalProductMatcher, NullLocalProductMatcher>();
        services.TryAddScoped<ILocalProductCatalogReader, NullLocalProductCatalogReader>();

        services.AddHttpClient<ZidProvider>(client =>
        {
            client.BaseAddress = new Uri("https://api.zid.sa/");
            client.Timeout = TimeSpan.FromSeconds(60);
        });

        services.AddHttpClient<SallaProvider>(client =>
        {
            client.BaseAddress = new Uri("https://api.salla.dev/");
            client.Timeout = TimeSpan.FromSeconds(60);
        });

        services.AddScoped<IECommerceProvider>(provider =>
            provider.GetRequiredService<ZidProvider>());

        services.AddScoped<IECommerceProvider>(provider =>
            provider.GetRequiredService<SallaProvider>());

        return services;
    }
}
