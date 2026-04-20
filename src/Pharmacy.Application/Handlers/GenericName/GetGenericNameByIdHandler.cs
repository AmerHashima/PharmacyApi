using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.GenericName;
using Pharmacy.Application.Queries.GenericName;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.GenericName;

public class GetGenericNameByIdHandler : IRequestHandler<GetGenericNameByIdQuery, GenericNameDto?>
{
    private readonly IGenericNameRepository _repository;
    private readonly IMapper _mapper;

    public GetGenericNameByIdHandler(IGenericNameRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GenericNameDto?> Handle(GetGenericNameByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return entity is null ? null : _mapper.Map<GenericNameDto>(entity);
    }
}
