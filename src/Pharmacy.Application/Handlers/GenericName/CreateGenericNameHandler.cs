using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.GenericName;
using Pharmacy.Application.DTOs.GenericName;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.GenericName;

public class CreateGenericNameHandler : IRequestHandler<CreateGenericNameCommand, GenericNameDto>
{
    private readonly IGenericNameRepository _repository;
    private readonly IMapper _mapper;

    public CreateGenericNameHandler(IGenericNameRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GenericNameDto> Handle(CreateGenericNameCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.GenericName>(request.GenericName);
        entity.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<GenericNameDto>(entity);
    }
}
