using ECommerceIntegration.Application.Abstractions;
using MediatR;
using ECommerceIntegration.Application.Commands;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Application.Handlers;

public class CreateECommerceStoreHandler : IRequestHandler<CreateECommerceStoreCommand, ECommerceStore>
{
    private readonly IECommerceIntegrationDbContext _db;

    public CreateECommerceStoreHandler(IECommerceIntegrationDbContext db)
    {
        _db = db;
    }

    public async Task<ECommerceStore> Handle(
        CreateECommerceStoreCommand request,
        CancellationToken cancellationToken)
    {
        var store = new ECommerceStore
        {
            Oid = Guid.NewGuid(),
            ProviderType = request.ProviderType,
            ExternalStoreId = request.ExternalStoreId,
            StoreName = request.StoreName,
            AccessToken = request.AccessToken,
            RefreshToken = request.RefreshToken,
            TokenExpiresAt = request.TokenExpiresAt,
            BranchId = request.BranchId,
            WarehouseId = request.WarehouseId,
            PriceListId = request.PriceListId,
            AutoPullOrders = request.AutoPullOrders,
            AutoSyncStock = request.AutoSyncStock,
            AutoSyncPrice = request.AutoSyncPrice,
            IsActive = request.IsActive
        };

        _db.ECommerceStores.Add(store);
        await _db.SaveChangesAsync(cancellationToken);

        return store;
    }
}
