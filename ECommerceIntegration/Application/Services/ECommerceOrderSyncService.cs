using Microsoft.EntityFrameworkCore;
using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Application.Dtos;
using ECommerceIntegration.Domain.Entities;
using ECommerceIntegration.Domain.Enums;

namespace ECommerceIntegration.Application.Services;

public class ECommerceOrderSyncService : IECommerceOrderSyncService
{
    private readonly IECommerceIntegrationDbContext _db;
    private readonly IECommerceProviderFactory _providerFactory;

    public ECommerceOrderSyncService(
        IECommerceIntegrationDbContext db,
        IECommerceProviderFactory providerFactory)
    {
        _db = db;
        _providerFactory = providerFactory;
    }

    public async Task PullOrdersAsync(
        Guid storeId,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken)
    {
        var store = await GetStoreAsync(storeId, cancellationToken);
        var provider = _providerFactory.GetProvider(store.ProviderType);
        var orders = await provider.GetOrdersAsync(
            store.AccessToken!,
            fromDate,
            toDate,
            cancellationToken);

        foreach (var orderDto in orders)
        {
            await SaveOrderIfNewAsync(store, orderDto, cancellationToken);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task PullOrderByExternalIdAsync(
        Guid storeId,
        string externalOrderId,
        CancellationToken cancellationToken)
    {
        var store = await GetStoreAsync(storeId, cancellationToken);
        var provider = _providerFactory.GetProvider(store.ProviderType);
        var orderDto = await provider.GetOrderByIdAsync(
            store.AccessToken!,
            externalOrderId,
            cancellationToken);

        if (orderDto == null)
        {
            return;
        }

        await SaveOrderIfNewAsync(store, orderDto, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task<ECommerceStore> GetStoreAsync(Guid storeId, CancellationToken cancellationToken)
    {
        var store = await _db.ECommerceStores
            .FirstOrDefaultAsync(x => x.Oid == storeId && !x.IsDeleted && x.IsActive, cancellationToken);

        if (store == null)
        {
            throw new InvalidOperationException("E-Commerce store not found.");
        }

        if (string.IsNullOrWhiteSpace(store.AccessToken))
        {
            throw new InvalidOperationException("E-Commerce store is not connected.");
        }

        return store;
    }

    private async Task SaveOrderIfNewAsync(
        ECommerceStore store,
        ECommerceOrderDto orderDto,
        CancellationToken cancellationToken)
    {
        var exists = await _db.ECommerceOrders.AnyAsync(
            x => x.ProviderType == store.ProviderType && x.ExternalOrderId == orderDto.ExternalOrderId,
            cancellationToken);

        if (exists)
        {
            return;
        }

        var order = new ECommerceOrder
        {
            Oid = Guid.NewGuid(),
            StoreId = store.Oid,
            ProviderType = store.ProviderType,
            ExternalOrderId = orderDto.ExternalOrderId,
            ExternalOrderNumber = orderDto.ExternalOrderNumber,
            CustomerName = orderDto.CustomerName,
            CustomerMobile = orderDto.CustomerMobile,
            CustomerEmail = orderDto.CustomerEmail,
            OrderStatus = orderDto.OrderStatus,
            PaymentStatus = orderDto.PaymentStatus,
            PaymentMethod = orderDto.PaymentMethod,
            SubTotal = orderDto.SubTotal,
            DiscountAmount = orderDto.DiscountAmount,
            ShippingAmount = orderDto.ShippingAmount,
            VatAmount = orderDto.VatAmount,
            TotalAmount = orderDto.TotalAmount,
            ExternalCreatedAt = orderDto.ExternalCreatedAt,
            SyncStatus = ECommerceSyncStatus.Pending
        };

        foreach (var itemDto in orderDto.Items)
        {
            var mapping = await FindMappingAsync(store.Oid, itemDto, cancellationToken);

            order.Items.Add(new ECommerceOrderItem
            {
                Oid = Guid.NewGuid(),
                ExternalProductId = itemDto.ExternalProductId,
                ExternalVariantId = itemDto.ExternalVariantId,
                ExternalSku = itemDto.ExternalSku,
                ExternalBarcode = itemDto.ExternalBarcode,
                ProductName = itemDto.ProductName,
                LocalProductId = mapping?.LocalProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                DiscountAmount = itemDto.DiscountAmount,
                VatAmount = itemDto.VatAmount,
                TotalAmount = itemDto.TotalAmount
            });
        }

        _db.ECommerceOrders.Add(order);
    }

    private async Task<ECommerceProductMapping?> FindMappingAsync(
        Guid storeId,
        ECommerceOrderItemDto itemDto,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(itemDto.ExternalProductId))
        {
            var exactMapping = await _db.ECommerceProductMappings.FirstOrDefaultAsync(
                x => x.StoreId == storeId
                     && x.ExternalProductId == itemDto.ExternalProductId
                     && x.ExternalVariantId == itemDto.ExternalVariantId
                     && x.IsActive,
                cancellationToken);

            if (exactMapping != null)
            {
                return exactMapping;
            }
        }

        if (string.IsNullOrWhiteSpace(itemDto.ExternalBarcode))
        {
            return null;
        }

        return await _db.ECommerceProductMappings.FirstOrDefaultAsync(
            x => x.StoreId == storeId
                 && x.ExternalBarcode == itemDto.ExternalBarcode
                 && x.IsActive,
            cancellationToken);
    }
}
