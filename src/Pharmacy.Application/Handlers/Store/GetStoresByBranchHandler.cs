using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Store;
using Pharmacy.Application.Queries.Store;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Store;

public class GetStoresByBranchHandler : IRequestHandler<GetStoresByBranchQuery, IEnumerable<StoreDto>>
{
    private readonly IStoreRepository _repository;
    private readonly IMapper _mapper;

    public GetStoresByBranchHandler(IStoreRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StoreDto>> Handle(GetStoresByBranchQuery request, CancellationToken cancellationToken)
    {
        var stores = await _repository.GetByBranchAsync(request.BranchId, cancellationToken);
        return _mapper.Map<IEnumerable<StoreDto>>(stores);
    }
}
