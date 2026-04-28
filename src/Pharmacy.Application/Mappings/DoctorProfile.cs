using AutoMapper;
using Pharmacy.Application.DTOs.Doctor;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

public class DoctorProfile : Profile
{
    public DoctorProfile()
    {
        CreateMap<Doctor, DoctorDto>()
            .ForMember(dest => dest.SpecialtyName,
                opt => opt.MapFrom(src => src.Specialty != null ? src.Specialty.ValueNameEn : null))
            .ForMember(dest => dest.SpecialtyNameAr,
                opt => opt.MapFrom(src => src.Specialty != null ? src.Specialty.ValueNameAr : null))
            .ForMember(dest => dest.ReferralTypeName,
                opt => opt.MapFrom(src => src.ReferralType != null ? src.ReferralType.ValueNameEn : null))
            .ForMember(dest => dest.ReferralTypeNameAr,
                opt => opt.MapFrom(src => src.ReferralType != null ? src.ReferralType.ValueNameAr : null))
            .ForMember(dest => dest.IdentityTypeName,
                opt => opt.MapFrom(src => src.IdentityType != null ? src.IdentityType.ValueNameEn : null));

        CreateMap<CreateDoctorDto, Doctor>();

        CreateMap<UpdateDoctorDto, Doctor>()
            .ForMember(dest => dest.Oid,       opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt,  opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy,  opt => opt.Ignore());
    }
}
