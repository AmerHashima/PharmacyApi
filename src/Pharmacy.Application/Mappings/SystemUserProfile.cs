using AutoMapper;
using Pharmacy.Application.DTOs.SystemUserSpace;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class SystemUserProfile : Profile
{
    public SystemUserProfile()
    {
        CreateMap<SystemUser, SystemUserDto>()
            .ForMember(dest => dest.RoleName,           opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null))
            .ForMember(dest => dest.GenderName,         opt => opt.MapFrom(src => src.Gender != null ? src.Gender.ValueNameEn : null))
            .ForMember(dest => dest.DefaultBranchName,  opt => opt.MapFrom(src => src.DefaultBranch != null ? src.DefaultBranch.BranchName : null));

        CreateMap<SystemUserDto, SystemUser>()
            .ForMember(dest => dest.DefaultBranch, opt => opt.Ignore());

        CreateMap<CreateSystemUserDto, SystemUser>();

        CreateMap<UpdateSystemUserDto, SystemUser>()
            ;
    }
}