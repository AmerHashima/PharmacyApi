using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.Rsd;
using Pharmacy.Application.Queries.Rsd;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.Rsd;

public class GetRsdOperationLogByIdHandler : IRequestHandler<GetRsdOperationLogByIdQuery, RsdOperationLogWithDetailsDto?>
{
    private readonly IRsdOperationLogRepository _repository;
    private readonly IMapper _mapper;

    public GetRsdOperationLogByIdHandler(IRsdOperationLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RsdOperationLogWithDetailsDto?> Handle(GetRsdOperationLogByIdQuery request, CancellationToken cancellationToken)
    {
        var log = await _repository.GetWithDetailsAsync(request.Id, cancellationToken);
        return log == null ? null : _mapper.Map<RsdOperationLogWithDetailsDto>(log);
    }
}
