using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, AccountDto>
{
    private readonly IAccountRepository _repository;
    private readonly IMapper _mapper;

    public UpdateAccountHandler(IAccountRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Account.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"Account '{request.Account.Oid}' not found");

        _mapper.Map(request.Account, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(entity, cancellationToken);
        return _mapper.Map<AccountDto>(entity);
    }
}
