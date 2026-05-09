using AutoMapper;
using MediatR;
using Pharmacy.Application.Commands.Accounting;
using Pharmacy.Application.DTOs.Accounting;
using Pharmacy.Domain.Interfaces.Accounting;

namespace Pharmacy.Application.Handlers.Accounting;

public class UpdateBankAccountHandler : IRequestHandler<UpdateBankAccountCommand, BankAccountDto>
{
    private readonly IBankAccountRepository _repository;
    private readonly IMapper _mapper;

    public UpdateBankAccountHandler(IBankAccountRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BankAccountDto> Handle(UpdateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.BankAccount.Oid, cancellationToken)
            ?? throw new KeyNotFoundException($"BankAccount '{request.BankAccount.Oid}' not found");

        _mapper.Map(request.BankAccount, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(entity, cancellationToken);
        return _mapper.Map<BankAccountDto>(entity);
    }
}
