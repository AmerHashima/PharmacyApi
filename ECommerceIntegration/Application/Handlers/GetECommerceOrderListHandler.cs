using ECommerceIntegration.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerceIntegration.Application.Queries;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Application.Handlers;

public class GetECommerceOrderListHandler : IRequestHandler<GetECommerceOrderListQuery, IEnumerable<ECommerceOrder>>
{
    private readonly IECommerceIntegrationDbContext _db;

    public GetECommerceOrderListHandler(IECommerceIntegrationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ECommerceOrder>> Handle(
        GetECommerceOrderListQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.ECommerceOrders
            .AsNoTracking()
            .Include(x => x.Items)
            .Where(x => !x.IsDeleted);

        if (request.StoreId.HasValue)
        {
            query = query.Where(x => x.StoreId == request.StoreId.Value);
        }

        return await query
            .OrderByDescending(x => x.ExternalCreatedAt ?? x.CreatedAt)
            .Take(200)
            .ToListAsync(cancellationToken);
    }
}
