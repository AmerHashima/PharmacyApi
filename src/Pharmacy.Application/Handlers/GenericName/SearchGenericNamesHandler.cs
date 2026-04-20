using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.GenericName;
using Pharmacy.Application.Queries.GenericName;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.GenericName;

public class SearchGenericNamesHandler : IRequestHandler<SearchGenericNamesQuery, IEnumerable<GenericNameDto>>
{
    private readonly IGenericNameRepository _repository;
    private readonly IMapper _mapper;

    public SearchGenericNamesHandler(IGenericNameRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GenericNameDto>> Handle(SearchGenericNamesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.SearchAsync(request.Term, cancellationToken);
        return _mapper.Map<IEnumerable<GenericNameDto>>(entities);
    }
}
