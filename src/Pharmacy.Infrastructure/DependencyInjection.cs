using Pharmacy.Application.Interfaces;
using Pharmacy.Application.Mappings;
using Pharmacy.Application.Options;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Persistence;
using Pharmacy.Infrastructure.Repositories;
using Pharmacy.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pharmacy.Infrastructure.Integration.Rsd;
using Pharmacy.Infrastructure.Integration.Zatca;

namespace Pharmacy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string contentRootPath)
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
        // File Storage Options
        // ====================================
        services.Configure<FileStorageOptions>(opts =>
        {
            var section = configuration.GetSection(FileStorageOptions.SectionName);
            opts.AllowedExtensions = section.GetSection("AllowedExtensions").Get<string[]>()
                                     ?? opts.AllowedExtensions;
            opts.MaxFileSizeBytes = section.GetValue<long?>("MaxFileSizeBytes") ?? opts.MaxFileSizeBytes;

            // Resolve physical upload root from configured relative path
            var configured = section["UploadPath"] ?? "wwwroot/uploads";
            opts.UploadRootPath = Path.IsPathRooted(configured)
                ? configured
                : Path.Combine(contentRootPath, configured);
        });

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
        services.AddScoped<IStoreRepository, StoreRepository>();
        services.AddScoped<IInvoiceShapeRepository, InvoiceShapeRepository>();
        services.AddScoped<IInvoiceSetupRepository, InvoiceSetupRepository>();

        // ====================================
        // Register repositories - Products & Inventory
        // ====================================
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductBatchRepository, ProductBatchRepository>();
        services.AddScoped<IProductUnitRepository, ProductUnitRepository>();
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
        services.AddScoped<IStockTransactionDetailRepository, StockTransactionDetailRepository>();

        // ====================================
        // Register repositories - Sales & POS
        // ====================================
        services.AddScoped<ISalesInvoiceRepository, SalesInvoiceRepository>();
        services.AddScoped<ISalesInvoiceItemRepository, SalesInvoiceItemRepository>();

        // ====================================
        // Register repositories - Returns & Refunds
        // ====================================
        services.AddScoped<IReturnInvoiceRepository, ReturnInvoiceRepository>();
        services.AddScoped<IReturnInvoiceItemRepository, ReturnInvoiceItemRepository>();

        // ====================================
        // Register repositories - Stock Transaction Returns
        // ====================================
        services.AddScoped<IStockTransactionReturnRepository, StockTransactionReturnRepository>();
        services.AddScoped<IStockTransactionReturnDetailRepository, StockTransactionReturnDetailRepository>();

        // ====================================
        // Register repositories - Integrations
        // ====================================
        services.AddScoped<IIntegrationProviderRepository, IntegrationProviderRepository>();
        services.AddScoped<IBranchIntegrationSettingRepository, BranchIntegrationSettingRepository>();
        services.AddScoped<IRsdOperationLogRepository, RsdOperationLogRepository>();

        // ====================================
        // Register AutoMapper
        // ====================================
        services.AddAutoMapper(cfg => {
            // Optional: Add custom mapper configurations here
        }, typeof(SystemUserProfile).Assembly, typeof(InvoiceShapeProfile).Assembly);

        // ====================================
        // Register Infrastructure Services
        // ====================================
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IQueryBuilderService, QueryBuilderService>();
        services.AddScoped<IBarcodeParserService, BarcodeParserService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IInvoiceNumberService, InvoiceNumberService>();

        // Drug sync job tracker — singleton so it survives across request scopes
        services.AddSingleton<IDrugListSyncTracker, DrugListSyncTracker>();

        // HttpClient for RSD integration
        services.AddHttpClient("RsdClient", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // RSD Integration Service
        services.AddScoped<IRsdIntegrationService, RsdIntegrationService>();

        // HttpClient for ZATCA integration
        services.AddHttpClient("ZatcaClient", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(60);
        });

        // ZATCA Integration Service
        services.AddScoped<IZatcaIntegrationService, ZatcaIntegrationService>();

        return services;
    }
}