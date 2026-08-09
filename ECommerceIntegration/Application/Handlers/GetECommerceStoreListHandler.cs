using ECommerceIntegration.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerceIntegration.Application.Queries;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Application.Handlers;

public class GetECommerceStoreListHandler : IRequestHandler<GetECommerceStoreListQuery, IEnumerable<ECommerceStore>>
{
    private readonly IECommerceIntegrationDbContext _db;

    public GetECommerceStoreListHandler(IECommerceIntegrationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ECommerceStore>> Handle(
        GetECommerceStoreListQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.ECommerceStores
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.StoreName)
            .ToListAsync(cancellationToken);
    }
}
