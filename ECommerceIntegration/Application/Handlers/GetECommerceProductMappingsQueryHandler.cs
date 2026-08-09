using ECommerceIntegration.Application.Abstractions;
using ECommerceIntegration.Application.Queries;
using ECommerceIntegration.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceIntegration.Application.Handlers;

public class GetECommerceProductMappingsQueryHandler
    : IRequestHandler<GetECommerceProductMappingsQuery, IReadOnlyList<ECommerceProductMapping>>
{
    private readonly IECommerceIntegrationDbContext _db;

    public GetECommerceProductMappingsQueryHandler(IECommerceIntegrationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ECommerceProductMapping>> Handle(
        GetECommerceProductMappingsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.ECommerceProductMappings
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (request.StoreId.HasValue)
        {
            query = query.Where(x => x.StoreId == request.StoreId.Value);
        }

        return await query
            .OrderBy(x => x.ExternalProductName)
            .ThenBy(x => x.Oid)
            .ToListAsync(cancellationToken);
    }
}
