using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.DTOs.Common;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Application.Services;
using Pharmacy.Domain.Entities.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

// ── Query: paged list ────────────────────────────────────────────────────────
public class GetAccountingSettingsDataHandler
    : IRequestHandler<GetAccountingSettingsDataQuery, PagedResult<AccountingSettingsDto>>
{
    private readonly IAccountingSettingsRepository _repository;
    private readonly IQueryBuilderService _queryBuilder;
    private readonly IMapper _mapper;

    public GetAccountingSettingsDataHandler(
        IAccountingSettingsRepository repository,
        IQueryBuilderService queryBuilder,
        IMapper mapper)
    {
        _repository   = repository;
        _queryBuilder = queryBuilder;
        _mapper       = mapper;
    }

    public async Task<PagedResult<AccountingSettingsDto>> Handle(
        GetAccountingSettingsDataQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(s => s.Branch)
            .Include(s => s.SalesAccount)
            .Include(s => s.VatAccount)
            .Include(s => s.SalesDiscountAccount)
            .Include(s => s.CogsAccount)
            .Include(s => s.InventoryAccount)
            .Include(s => s.CashAccount)
            .Include(s => s.BankAccount)
            .Include(s => s.ReceivableAccount)
            .Where(s => !s.IsDeleted);

        var paged = await _queryBuilder.ExecuteQueryAsync(query, request.QueryRequest.Request);
        return new PagedResult<AccountingSettingsDto>
        {
            Data            = _mapper.Map<IEnumerable<AccountingSettingsDto>>(paged.Data),
            TotalRecords    = paged.TotalRecords,
            PageNumber      = paged.PageNumber,
            PageSize        = paged.PageSize,
            TotalPages      = paged.TotalPages,
            HasNextPage     = paged.HasNextPage,
            HasPreviousPage = paged.HasPreviousPage
        };
    }
}

// ── Query: by ID ─────────────────────────────────────────────────────────────
public class GetAccountingSettingsByIdHandler
    : IRequestHandler<GetAccountingSettingsByIdQuery, AccountingSettingsDto?>
{
    private readonly IAccountingSettingsRepository _repository;
    private readonly IMapper _mapper;

    public GetAccountingSettingsByIdHandler(IAccountingSettingsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<AccountingSettingsDto?> Handle(
        GetAccountingSettingsByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByBranchAsync(request.Id, cancellationToken)
                  ?? await _repository.GetByIdAsync(request.Id, cancellationToken);
        return entity is null ? null : _mapper.Map<AccountingSettingsDto>(entity);
    }
}

// ── Query: by Branch ─────────────────────────────────────────────────────────
public class GetAccountingSettingsByBranchHandler
    : IRequestHandler<GetAccountingSettingsByBranchQuery, AccountingSettingsDto?>
{
    private readonly IAccountingSettingsRepository _repository;
    private readonly IMapper _mapper;

    public GetAccountingSettingsByBranchHandler(IAccountingSettingsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<AccountingSettingsDto?> Handle(
        GetAccountingSettingsByBranchQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByBranchAsync(request.BranchId, cancellationToken);
        return entity is null ? null : _mapper.Map<AccountingSettingsDto>(entity);
    }
}

// ── Command: Create ──────────────────────────────────────────────────────────
public class CreateAccountingSettingsHandler
    : IRequestHandler<CreateAccountingSettingsCommand, AccountingSettingsDto>
{
    private readonly IAccountingSettingsRepository _repository;
    private readonly IMapper _mapper;

    public CreateAccountingSettingsHandler(IAccountingSettingsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<AccountingSettingsDto> Handle(
        CreateAccountingSettingsCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByBranchAsync(request.Settings.BranchId, cancellationToken);
        if (existing != null)
            throw new InvalidOperationException(
                $"Accounting settings for branch '{request.Settings.BranchId}' already exist. Use update instead.");

        var entity = _mapper.Map<AccountingSettings>(request.Settings);
        entity.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(entity, cancellationToken);

        var created = await _repository.GetByBranchAsync(entity.BranchId, cancellationToken);
        return _mapper.Map<AccountingSettingsDto>(created);
    }
}

// ── Command: Update ──────────────────────────────────────────────────────────
public class UpdateAccountingSettingsHandler
    : IRequestHandler<UpdateAccountingSettingsCommand, AccountingSettingsDto>
{
    private readonly IAccountingSettingsRepository _repository;
    private readonly IMapper _mapper;

    public UpdateAccountingSettingsHandler(IAccountingSettingsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<AccountingSettingsDto> Handle(
        UpdateAccountingSettingsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Settings.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"Accounting settings '{request.Settings.Oid}' not found.");

        _mapper.Map(request.Settings, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(entity, cancellationToken);

        var updated = await _repository.GetByBranchAsync(entity.BranchId, cancellationToken);
        return _mapper.Map<AccountingSettingsDto>(updated);
    }
}

// ── Command: Delete ──────────────────────────────────────────────────────────
public class DeleteAccountingSettingsHandler
    : IRequestHandler<DeleteAccountingSettingsCommand, bool>
{
    private readonly IAccountingSettingsRepository _repository;

    public DeleteAccountingSettingsHandler(IAccountingSettingsRepository repository)
        => _repository = repository;

    public async Task<bool> Handle(
        DeleteAccountingSettingsCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.ExistsAsync(request.Id, cancellationToken))
            return false;
        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
