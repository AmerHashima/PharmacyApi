using Pharmacy.Application.Interfaces;
using Pharmacy.Application.Mappings;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;
using Pharmacy.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pharmacy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<PharmacyDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)));

        // ====================================
        // Register repositories - Core System
        // ====================================
        services.AddScoped<ISystemUserRepository, SystemUserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        // ====================================
        // Register repositories - Lookups
        // ====================================
        services.AddScoped<IAppLookupMasterRepository, AppLookupMasterRepository>();
        services.AddScoped<IAppLookupDetailRepository, AppLookupDetailRepository>();

        // ====================================
        // Register repositories - Pharmacy Structure
        // ====================================
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IStakeholderRepository, StakeholderRepository>();

        // ====================================
        // Register repositories - Products & Inventory
        // ====================================
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductBatchRepository, ProductBatchRepository>();
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
        services.AddScoped<IStockTransactionDetailRepository, StockTransactionDetailRepository>();

        // ====================================
        // Register repositories - Sales & POS
        // ====================================
        services.AddScoped<ISalesInvoiceRepository, SalesInvoiceRepository>();
        services.AddScoped<ISalesInvoiceItemRepository, SalesInvoiceItemRepository>();

        // ====================================
        // Register repositories - Integrations
        // ====================================
        services.AddScoped<IIntegrationProviderRepository, IntegrationProviderRepository>();
        services.AddScoped<IBranchIntegrationSettingRepository, BranchIntegrationSettingRepository>();

        // ====================================
        // Register AutoMapper
        // ====================================
        services.AddAutoMapper(cfg => {
            // Optional: Add custom mapper configurations here
        }, typeof(SystemUserProfile).Assembly);

        // ====================================
        // Register Infrastructure Services
        // ====================================
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IQueryBuilderService, QueryBuilderService>();
        services.AddScoped<IBarcodeParserService, BarcodeParserService>();

        return services;
    }
}