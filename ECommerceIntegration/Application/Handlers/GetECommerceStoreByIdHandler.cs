using ECommerceIntegration.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerceIntegration.Application.Queries;
using ECommerceIntegration.Domain.Entities;

namespace ECommerceIntegration.Application.Handlers;

public class GetECommerceStoreByIdHandler : IRequestHandler<GetECommerceStoreByIdQuery, ECommerceStore?>
{
    private readonly IECommerceIntegrationDbContext _db;

    public GetECommerceStoreByIdHandler(IECommerceIntegrationDbContext db)
    {
        _db = db;
    }

    public async Task<ECommerceStore?> Handle(
        GetECommerceStoreByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.ECommerceStores
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Oid == request.Id && !x.IsDeleted, cancellationToken);
    }
}
