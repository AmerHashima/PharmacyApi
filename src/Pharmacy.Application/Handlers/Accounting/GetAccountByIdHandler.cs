using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, AccountDto?>
{
    private readonly IAccountRepository _repository;
    private readonly IMapper _mapper;

    public GetAccountByIdHandler(IAccountRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AccountDto?> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetQueryable()
            .Include(a => a.Parent)
            .Include(a => a.AccountType)
            .Include(a => a.Nature)
            .Where(a => a.Oid == request.Id && !a.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        return entity is null ? null : _mapper.Map<AccountDto>(entity);
    }
}
