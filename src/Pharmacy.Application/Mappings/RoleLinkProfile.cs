using AutoMapper;
using Pharmacy.Application.DTOs.RoleLink;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class RoleLinkProfile : Profile
{
    public RoleLinkProfile()
    {
        CreateMap<RoleLink, RoleLinkDto>()
            .ForMember(d => d.RoleName,    opt => opt.MapFrom(s => s.Role != null ? s.Role.Name : null))
            .ForMember(d => d.LinkNameAr,  opt => opt.MapFrom(s => s.Link != null ? s.Link.NameAr : null))
            .ForMember(d => d.LinkNameEn,  opt => opt.MapFrom(s => s.Link != null ? s.Link.NameEn : null))
            .ForMember(d => d.LinkPath,    opt => opt.MapFrom(s => s.Link != null ? s.Link.Path : null));

        CreateMap<CreateRoleLinkDto, RoleLink>();
        CreateMap<UpdateRoleLinkDto, RoleLink>()
            .ForMember(d => d.Oid,       opt => opt.Ignore())
            .ForMember(d => d.RoleId,    opt => opt.Ignore())
            .ForMember(d => d.LinkId,    opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore());
    }
}
