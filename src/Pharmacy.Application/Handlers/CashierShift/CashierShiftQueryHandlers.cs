using AutoMapper;
using MediatR;
using Pharmacy.Application.DTOs.CashierShift;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.CashierShift;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Interfaces;

namespace Pharmacy.Application.Handlers.CashierShift;

public class GetCashierShiftByIdHandler : IRequestHandler<GetCashierShiftByIdQuery, CashierShiftWithDetailsDto?>
{
    private readonly ICashierShiftRepository _repository;
    private readonly IMapper _mapper;

    public GetCashierShiftByIdHandler(ICashierShiftRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CashierShiftWithDetailsDto?> Handle(GetCashierShiftByIdQuery request, CancellationToken cancellationToken)
    {
        var shift = await _repository.GetWithDetailsAsync(request.Id, cancellationToken);
        return shift == null ? null : _mapper.Map<CashierShiftWithDetailsDto>(shift);
    }
}

public class GetCashierShiftDataHandler : IRequestHandler<GetCashierShiftDataQuery, PagedResult<CashierShiftDto>>
{
    private readonly ICashierShiftRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetCashierShiftDataHandler(ICashierShiftRepository repository, IQueryBuilderService queryBuilder, IMapper mapper)
    {
        _repository = repository;
        _queryBuilder = queryBuilder;
        _mapper = mapper;
    }

    public async Task<PagedResult<CashierShiftDto>> Handle(GetCashierShiftDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable().Where(x => !x.IsDeleted);
        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);
        var dtos = _mapper.Map<List<CashierShiftDto>>(paged.Data);
        return PagedResult<CashierShiftDto>.Create(dtos, paged.TotalRecords, paged.PageNumber, paged.PageSize);
    }
}

public class GetCashierShiftsByBranchHandler : IRequestHandler<GetCashierShiftsByBranchQuery, IEnumerable<CashierShiftDto>>
{
    private readonly ICashierShiftRepository _repository;
    private readonly IMapper _mapper;

    public GetCashierShiftsByBranchHandler(ICashierShiftRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CashierShiftDto>> Handle(GetCashierShiftsByBranchQuery request, CancellationToken cancellationToken)
    {
        var shifts = await _repository.GetByBranchAsync(request.BranchId, cancellationToken);
        return _mapper.Map<IEnumerable<CashierShiftDto>>(shifts);
    }
}

public class GetCashierShiftsByUserHandler : IRequestHandler<GetCashierShiftsByUserQuery, IEnumerable<CashierShiftDto>>
{
    private readonly ICashierShiftRepository _repository;
    private readonly IMapper _mapper;

    public GetCashierShiftsByUserHandler(ICashierShiftRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CashierShiftDto>> Handle(GetCashierShiftsByUserQuery request, CancellationToken cancellationToken)
    {
        var shifts = await _repository.GetByUserAsync(request.UserId, cancellationToken);
        return _mapper.Map<IEnumerable<CashierShiftDto>>(shifts);
    }
}

public class GetOpenShiftHandler : IRequestHandler<GetOpenShiftQuery, CashierShiftWithDetailsDto?>
{
    private readonly ICashierShiftRepository _repository;
    private readonly IMapper _mapper;

    public GetOpenShiftHandler(ICashierShiftRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CashierShiftWithDetailsDto?> Handle(GetOpenShiftQuery request, CancellationToken cancellationToken)
    {
        var shift = await _repository.GetOpenShiftAsync(request.BranchId, request.UserId, cancellationToken);
        return shift == null ? null : _mapper.Map<CashierShiftWithDetailsDto>(shift);
    }
}
