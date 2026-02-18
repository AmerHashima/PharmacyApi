using AutoMapper;
using Pharmacy.Application.DTOs.SystemUserSpace;
using Pharmacy.Application.Queries.SystemUserSpace;
using Pharmacy.Domain.Interfaces;
using MediatR;

namespace Pharmacy.Application.Handlers.SystemUserSpace;

public class GetSystemUserListHandler : IRequestHandler<GetSystemUserListQuery, IEnumerable<SystemUserDto>>
{
    private readonly ISystemUserRepository _repository;
    private readonly IMapper _mapper;

    public GetSystemUserListHandler(ISystemUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SystemUserDto>> Handle(GetSystemUserListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.SystemUser> users;

      if (!request.IncludeInactive)
        {
            users = await _repository.GetActiveUsersAsync(cancellationToken);
        }
        else
        {
            users = await _repository.GetAllAsync(cancellationToken);
        }

        return _mapper.Map<IEnumerable<SystemUserDto>>(users);
    }
}