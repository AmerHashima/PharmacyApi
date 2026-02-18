using AutoMapper;
using Pharmacy.Application.DTOs.AppLookup;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class AppLookupProfile : Profile
{
    public AppLookupProfile()
    {
        // AppLookupMaster mappings
        CreateMap<AppLookupMaster, AppLookupMasterDto>()
            .ReverseMap();

        CreateMap<CreateAppLookupMasterDto, AppLookupMaster>()
            .ForMember(dest => dest.Oid, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.LookupDetails, opt => opt.Ignore());

        // AppLookupDetail mappings
        CreateMap<AppLookupDetail, AppLookupDetailDto>()
            .ForMember(dest => dest.MasterLookupCode, opt => opt.MapFrom(src => src.Master.LookupCode))
            .ReverseMap();

        CreateMap<CreateAppLookupDetailDto, AppLookupDetail>()
            .ForMember(dest => dest.Oid, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Master    , opt => opt.Ignore());
    }
}