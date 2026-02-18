using AutoMapper;
using Pharmacy.Application.DTOs.Role;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
        
        CreateMap<CreateRoleDto, Role>()
            .ForMember(dest => dest.Oid, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Users, opt => opt.Ignore());

        CreateMap<UpdateRoleDto, Role>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Users, opt => opt.Ignore());
    }
}