using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTOs.SystemUserSpace;
using Pharmacy.Application.Queries.SystemUserSpace;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.SystemUserSpace;

public class GetSystemUserByIdHandler : IRequestHandler<GetSystemUserByIdQuery, SystemUserDto?>
{
    private readonly ISystemUserRepository _repository;
    private readonly IMapper _mapper;

    public GetSystemUserByIdHandler(ISystemUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SystemUserDto?> Handle(GetSystemUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetQueryable()
            .Include(x => x.Role)
            .Include(x => x.GenderLookup)
            .Include(x => x.DefaultBranch)
            .Where(x => x.Oid == request.Id && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        return user == null ? null : _mapper.Map<SystemUserDto>(user);
    }
}