using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Application.Queries.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class GetBankAccountByIdHandler : IRequestHandler<GetBankAccountByIdQuery, BankAccountDto?>
{
    private readonly IBankAccountRepository _repository;
    private readonly IMapper _mapper;

    public GetBankAccountByIdHandler(IBankAccountRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BankAccountDto?> Handle(GetBankAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetQueryable()
            .Include(ba => ba.Branch)
            .Include(ba => ba.ChildAccount)
            .Include(ba => ba.CurrencyCode)
            .Where(ba => ba.Oid == request.Id && !ba.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        return entity is null ? null : _mapper.Map<BankAccountDto>(entity);
    }
}
