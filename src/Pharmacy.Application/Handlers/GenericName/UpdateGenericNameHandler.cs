using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.GenericName;
using Pharmacy.Application.DTOs.GenericName;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.GenericName;

public class UpdateGenericNameHandler : IRequestHandler<UpdateGenericNameCommand, GenericNameDto>
{
    private readonly IGenericNameRepository _repository;
    private readonly IMapper _mapper;

    public UpdateGenericNameHandler(IGenericNameRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GenericNameDto> Handle(UpdateGenericNameCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.GenericName.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"GenericName with ID '{request.GenericName.Oid}' not found");

        _mapper.Map(request.GenericName, entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(entity, cancellationToken);
        return _mapper.Map<GenericNameDto>(entity);
    }
}
