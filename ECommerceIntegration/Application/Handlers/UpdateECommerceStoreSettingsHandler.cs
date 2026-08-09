using ECommerceIntegration.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerceIntegration.Application.Commands;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Application.Handlers;

public class UpdateECommerceStoreSettingsHandler : IRequestHandler<UpdateECommerceStoreSettingsCommand, ECommerceStore?>
{
    private readonly IECommerceIntegrationDbContext _db;

    public UpdateECommerceStoreSettingsHandler(IECommerceIntegrationDbContext db)
    {
        _db = db;
    }

    public async Task<ECommerceStore?> Handle(
        UpdateECommerceStoreSettingsCommand request,
        CancellationToken cancellationToken)
    {
        var store = await _db.ECommerceStores
            .FirstOrDefaultAsync(x => x.Oid == request.Id && !x.IsDeleted, cancellationToken);

        if (store == null)
        {
            return null;
        }

        store.BranchId = request.BranchId;
        store.WarehouseId = request.WarehouseId;
        store.PriceListId = request.PriceListId;
        store.AutoPullOrders = request.AutoPullOrders;
        store.AutoSyncStock = request.AutoSyncStock;
        store.AutoSyncPrice = request.AutoSyncPrice;
        store.IsActive = request.IsActive;

        await _db.SaveChangesAsync(cancellationToken);
        return store;
    }
}
