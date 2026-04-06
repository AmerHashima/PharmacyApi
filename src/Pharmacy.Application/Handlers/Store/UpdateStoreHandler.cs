using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Commands.Store;
using Pharmacy.Application.DTOs.Store;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Store;

public class UpdateStoreHandler : IRequestHandler<UpdateStoreCommand, StoreDto>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IMapper _mapper;

    public UpdateStoreHandler(IStoreRepository storeRepository, IMapper mapper)
    {
        _storeRepository = storeRepository;
        _mapper = mapper;
    }

    public async Task<StoreDto> Handle(UpdateStoreCommand request, CancellationToken cancellationToken)
    {
        var existing = await _storeRepository.GetByIdAsync(request.Store.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"Store with ID '{request.Store.Oid}' not found");

        if (await _storeRepository.CodeExistsAsync(request.Store.StoreCode, request.Store.Oid, cancellationToken))
            throw new InvalidOperationException($"Store with code '{request.Store.StoreCode}' already exists");

        _mapper.Map(request.Store, existing);
        await _storeRepository.UpdateAsync(existing, cancellationToken);

        var result = await _storeRepository.GetQueryable()
            .Include(s => s.Branch)
            .FirstOrDefaultAsync(s => s.Oid == existing.Oid, cancellationToken);

        return _mapper.Map<StoreDto>(result);
    }
}
