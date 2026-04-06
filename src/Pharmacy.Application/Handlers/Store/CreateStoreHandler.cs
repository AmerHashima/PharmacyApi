using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Commands.Store;
using Pharmacy.Application.DTOs.Store;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Store;

public class CreateStoreHandler : IRequestHandler<CreateStoreCommand, StoreDto>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public CreateStoreHandler(IStoreRepository storeRepository, IBranchRepository branchRepository, IMapper mapper)
    {
        _storeRepository = storeRepository;
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public async Task<StoreDto> Handle(CreateStoreCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Store.BranchId, cancellationToken)
            ?? throw new KeyNotFoundException($"Branch with ID '{request.Store.BranchId}' not found");

        if (await _storeRepository.CodeExistsAsync(request.Store.StoreCode, cancellationToken: cancellationToken))
            throw new InvalidOperationException($"Store with code '{request.Store.StoreCode}' already exists");

        var entity = _mapper.Map<Domain.Entities.Store>(request.Store);
        var created = await _storeRepository.AddAsync(entity, cancellationToken);

        var result = await _storeRepository.GetQueryable()
            .Include(s => s.Branch)
            .FirstOrDefaultAsync(s => s.Oid == created.Oid, cancellationToken);

        return _mapper.Map<StoreDto>(result);
    }
}
