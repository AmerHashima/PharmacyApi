using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Store;
using Pharmacy.Application.Queries.Store;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Store;

public class GetStoreByIdHandler : IRequestHandler<GetStoreByIdQuery, StoreDto?>
{
    private readonly IStoreRepository _repository;
    private readonly IMapper _mapper;

    public GetStoreByIdHandler(IStoreRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StoreDto?> Handle(GetStoreByIdQuery request, CancellationToken cancellationToken)
    {
        var store = await _repository.GetQueryable()
            .Include(s => s.Branch)
            .FirstOrDefaultAsync(s => s.Oid == request.Id && !s.IsDeleted, cancellationToken);

        return store == null ? null : _mapper.Map<StoreDto>(store);
    }
}
