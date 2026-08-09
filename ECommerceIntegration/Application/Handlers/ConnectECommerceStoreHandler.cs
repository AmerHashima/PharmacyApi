using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerceIntegration.Application.Commands;
using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Application.Handlers;

public class ConnectECommerceStoreHandler : IRequestHandler<ConnectECommerceStoreCommand, ECommerceStore>
{
    private readonly IECommerceIntegrationDbContext _db;
    private readonly IECommerceProviderFactory _providerFactory;

    public ConnectECommerceStoreHandler(
        IECommerceIntegrationDbContext db,
        IECommerceProviderFactory providerFactory)
    {
        _db = db;
        _providerFactory = providerFactory;
    }

    public async Task<ECommerceStore> Handle(
        ConnectECommerceStoreCommand request,
        CancellationToken cancellationToken)
    {
        var provider = _providerFactory.GetProvider(request.ProviderType);
        var token = await provider.ExchangeCodeAsync(request.Code, cancellationToken);

        var store = await _db.ECommerceStores.FirstOrDefaultAsync(
            x => x.ProviderType == request.ProviderType
                 && x.ExternalStoreId == token.ExternalStoreId
                 && !x.IsDeleted,
            cancellationToken);

        if (store == null)
        {
            store = new ECommerceStore
            {
                Oid = Guid.NewGuid(),
                ProviderType = request.ProviderType,
                ExternalStoreId = token.ExternalStoreId,
                BranchId = request.BranchId,
                WarehouseId = request.WarehouseId,
                PriceListId = request.PriceListId,
                IsActive = true
            };

            _db.ECommerceStores.Add(store);
        }

        store.StoreName = token.StoreName;
        store.AccessToken = token.AccessToken;
        store.RefreshToken = token.RefreshToken;
        store.TokenExpiresAt = token.ExpiresAt;

        await _db.SaveChangesAsync(cancellationToken);
        return store;
    }
}
