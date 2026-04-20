using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.GenericName;
using Pharmacy.Application.Queries.GenericName;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.GenericName;

public class GetAllGenericNamesHandler : IRequestHandler<GetAllGenericNamesQuery, IEnumerable<GenericNameDto>>
{
    private readonly IGenericNameRepository _repository;
    private readonly IMapper _mapper;

    public GetAllGenericNamesHandler(IGenericNameRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GenericNameDto>> Handle(GetAllGenericNamesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<GenericNameDto>>(entities);
    }
}
