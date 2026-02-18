using AutoMapper;
using Pharmacy.Application.DTOs.SystemUserSpace;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class SystemUserProfile : Profile
{
    public SystemUserProfile()
    {
        CreateMap<SystemUser, SystemUserDto>()
            .ReverseMap();

        CreateMap<CreateSystemUserDto, SystemUser>();

        CreateMap<UpdateSystemUserDto, SystemUser>()
            ;
    }
}