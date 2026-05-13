using AutoMapper;
using Pharmacy.Application.DTOs.Stakeholder;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Mappings;

/// <summary>
/// AutoMapper profile for Stakeholder entity mappings
/// </summary>
public class StakeholderProfile : Profile
{
    public StakeholderProfile()
    {
        // Entity to DTO
        CreateMap<Stakeholder, StakeholderDto>()
            .ForMember(dest => dest.StakeholderTypeName, 
                opt => opt.MapFrom(src => src.StakeholderType != null ? src.StakeholderType.ValueNameEn : null))
            .ForMember(dest => dest.ParentAccountName,
                opt => opt.MapFrom(src => src.ParentAccount != null ? src.ParentAccount.AccountNameAr : null))
            .ForMember(dest => dest.ChildAccountName,
                opt => opt.MapFrom(src => src.ChildAccount != null ? src.ChildAccount.AccountNameAr : null));

        // Create DTO to Entity
        CreateMap<CreateStakeholderDto, Stakeholder>();

        // Update DTO to Entity
        CreateMap<UpdateStakeholderDto, Stakeholder>()
            .ForMember(dest => dest.Oid, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
    }
}
